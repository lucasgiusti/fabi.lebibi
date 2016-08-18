app.controller('clienteController', function ($scope, $http, $window, toasterAlert, $location, $uibModal, $routeParams, UserService) {
    UserService.verificaLogin();

    var mensagemExcluir = 'Deseja realmente excluir o cliente [NOMECLIENTE] ?';
    var mensagemSalvo = JSON.stringify({ Success: "info", Messages: [{ Message: 'Cliente salvo com sucesso' }] });
    var url = 'api/cliente';
    var headerAuth = { headers: { 'Authorization': 'Basic ' + UserService.getUser().token } };

    $scope.heading = 'Clientes';
    $scope.clientes = [];
    $scope.cliente = null;

    //APIs
    $scope.getClientes = function () {

        $http.get(url, headerAuth).success(function (data) {
            $scope.clientes = data;
            $scope.total = $scope.clientes.length;
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        })
    };

    $scope.getCliente = function () {
        if (!angular.isUndefined($routeParams.id)) {
            $scope.id = $routeParams.id;
        }

        $http.get(url + '/' + $scope.id, headerAuth).success(function (data) {
            $scope.cliente = data;
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.postCliente = function () {

        $http.post(url, JSON.stringify($scope.cliente), headerAuth).success(function (id) {
            $scope.id = id;
            $scope.getCliente();
            toasterAlert.showAlert(mensagemSalvo);
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.putCliente = function () {
        $http.put(url + '/' + $scope.id, JSON.stringify($scope.cliente), headerAuth).success(function (data) {
            $scope.cliente = data;
            toasterAlert.showAlert(mensagemSalvo);
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    $scope.deleteCliente = function () {

        $http.delete(url + '/' + $scope.cliente.id, headerAuth).success(function (result) {
            toasterAlert.showAlert(result);
            $scope.clientes.splice($scope.clientes.indexOf($scope.cliente), 1);
        }).error(function (result) {
            toasterAlert.showAlert(result);
        });
    };

    //Utils
    $scope.addCliente = function () {
        $scope.cliente = {};
    };

    $scope.openModalDelete = function (cliente) {
        $scope.cliente = cliente;
        $scope.dadosModalConfirm = { 'titulo': 'Excluir', 'mensagem': mensagemExcluir.replace('[NOMECLIENTE]', $scope.cliente.nome) };

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
            $scope.deleteCliente();
        });
    };

    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    }
    spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

    $('.telefone').mask(SPMaskBehavior, spOptions);

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