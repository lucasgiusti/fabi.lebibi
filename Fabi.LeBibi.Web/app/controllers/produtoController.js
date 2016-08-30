app.controller('produtoController', function ($scope, $http, $window, toasterAlert, $location, $uibModal, $routeParams, UserService) {
    UserService.verificaLogin();

    var mensagemExcluir = 'Deseja realmente excluir o produto [CODIGOPRODUTO] ?';
    var mensagemSalvo = JSON.stringify({ Success: "info", Messages: [{ Message: 'Produto salvo com sucesso' }] });
    var url = 'api/produto';
    var headerAuth = { headers: { 'Authorization': 'Basic ' + UserService.getUser().token } };

    $scope.heading = 'Produtos';
    $scope.produtos = [];
    $scope.produto = null;

    //APIs
    $scope.getProdutos = function () {

        $http.get(url, headerAuth).success(function (data) {
            $scope.produtos = data;

            angular.forEach($scope.produtos, function (produto, key) {
                produto.valorVenda = produto.valorVenda.toFixed(2);
            });
            

            $scope.total = $scope.produtos.length;
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        })
    };

    $scope.getProduto = function () {
        if (!angular.isUndefined($routeParams.id)) {
            $scope.id = $routeParams.id;
        }

        $http.get(url + '/' + $scope.id, headerAuth).success(function (data) {
            $scope.produto = data;
            $scope.produto.valorCompra = $scope.produto.valorCompra.toFixed(2);
            $scope.produto.valorVenda = $scope.produto.valorVenda.toFixed(2);
            $scope.produto.margemLucro = $scope.produto.margemLucro.toFixed(2);
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.postProduto = function () {

        $http.post(url, JSON.stringify($scope.produto), headerAuth).success(function (id) {
            $scope.id = id;
            $scope.getProduto();
            toasterAlert.showAlert(mensagemSalvo);
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.putProduto = function () {
        $http.put(url + '/' + $scope.id, JSON.stringify($scope.produto), headerAuth).success(function (data) {
            $scope.produto = data;
            $scope.produto.valorCompra = $scope.produto.valorCompra.toFixed(2);
            $scope.produto.valorVenda = $scope.produto.valorVenda.toFixed(2);
            $scope.produto.margemLucro = $scope.produto.margemLucro.toFixed(2);
            toasterAlert.showAlert(mensagemSalvo);
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.deleteProduto = function () {

        $http.delete(url + '/' + $scope.produto.id, headerAuth).success(function (result) {
            toasterAlert.showAlert(result);
            $scope.produtos.splice($scope.produtos.indexOf($scope.produto), 1);
        }).error(function (result) {
            toasterAlert.showAlert(result);
        });
    };

    //Utils
    $scope.addProduto = function () {
        $scope.produto = {  };
    };

    $scope.openModalDelete = function (produto) {
        $scope.produto = produto;
        $scope.dadosModalConfirm = { 'titulo': 'Excluir', 'mensagem': mensagemExcluir.replace('[CODIGOPRODUTO]', $scope.produto.codigo) };

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'app/templates/modalConfirm.html',
            controller: 'modalConfirmInstanceController',
            resolve: {
                dadosModalConfirm: function () {
                    return $scope.dadosModalConfirm;
                }
            }
        });

        modalInstance.result.then(function () {
            $scope.deleteProduto();
        });
    };

    $scope.verificaValorCompraVazio = function () {
        if (!$scope.produto.valorCompra || ($scope.produto.valorCompra && $scope.produto.valorCompra == '')) {
            $scope.produto.valorVenda = $scope.produto.valorCompra;
        }
    };

    var options = {
        onChange: function (valor, event, currentField, options) {
            if ($(currentField).attr('class').indexOf('valorCompra') > -1) {
                if (!$scope.produto.margemLucro || $scope.produto.margemLucro == '') {
                    $scope.produto.valorVenda = valor;
                }
                else {
                    valor = findAndReplace(valor, ',', '');
                    var margemLucro = findAndReplace($scope.produto.margemLucro, ',', '');
                    $scope.produto.valorVenda = (((valor / 100) * margemLucro) + valor * 1).toFixed(2);
                    $scope.produto.valorVenda = $('.valorVenda').masked($scope.produto.valorVenda);
                }
            }
            if ($(currentField).attr('class').indexOf('margemLucro') > -1) {
                if ($scope.produto.valorCompra && $scope.produto.valorCompra != '') {
                    var valorCompra = findAndReplace($scope.produto.valorCompra, ',', '');
                    $scope.produto.valorVenda = null;
                    $scope.produto.valorVenda = (((valorCompra / 100) * valor) + valorCompra * 1).toFixed(2);
                    $scope.produto.valorVenda = $('.valorVenda').masked($scope.produto.valorVenda);
                }
            }
            if ($(currentField).attr('class').indexOf('valorVenda') > -1) {
                if ($scope.produto.valorCompra && $scope.produto.valorCompra != '') {
                    valor = findAndReplace(valor, ',', '');
                    var valorCompra = findAndReplace($scope.produto.valorCompra, ',', '');
                    var margemLucro = parseFloat(((valor - valorCompra) / valorCompra) * 100).toFixed(2)
                    if (margemLucro <= 0) {
                        margemLucro = null;
                    }
                    $scope.produto.margemLucro = margemLucro;
                }
            }
        },
        reverse: true
    };

    $('.money').mask('#,##0.00', options);
    $('.integer').mask('###0', { reverse: true });

    //PAGINATION
    $scope.total = 0;
    $scope.currentPage = 1;
    $scope.itemPerPage = 5;
    $scope.start = 0;
    $scope.maxSize = 5;
    $scope.pageChanged = function () {
        $scope.start = ($scope.currentPage - 1) * $scope.itemPerPage;
    };
});