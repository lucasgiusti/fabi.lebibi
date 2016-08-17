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
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.postProduto = function () {

        if ($scope.produto.valorCompra)
            $scope.produto.valorCompra = $scope.produto.valorCompra * 10;

        if ($scope.produto.margemLucro)
            $scope.produto.margemLucro = $scope.produto.margemLucro * 10;

        if ($scope.produto.valorVenda)
            $scope.produto.valorVenda = $scope.produto.valorVenda * 10;
        

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
