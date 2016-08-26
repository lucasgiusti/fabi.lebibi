app.controller('vendaController', function ($scope, $http, $window, toasterAlert, $location, $uibModal, $routeParams, UserService) {
    UserService.verificaLogin();

    var urlCliente = 'api/cliente';
    var urlProduto = 'api/produto';
    var headerAuth = { headers: { 'Authorization': 'Basic ' + UserService.getUser().token } };

    $scope.heading = 'Vendas';
    $scope.venda = null;
    $scope.cliente = null;
    $scope.produtos = [];
    $scope.totalPago = 0;

    //APIs
    $scope.getProdutos = function () {

        $http.get(urlProduto, headerAuth).success(function (data) {
            $scope.produtos = data;

            angular.forEach($scope.produtos, function (produto, key) {
                produto.valorVenda = produto.valorVenda.toFixed(2);
            });

        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        })
    };

    $scope.getCliente = function () {
        if (!angular.isUndefined($routeParams.id)) {
            $scope.id = $routeParams.id;
        }

        $http.get(urlCliente + '/' + $scope.id, headerAuth).success(function (data) {
            $scope.cliente = data;
        }).error(function (jqxhr, textStatus) {
            toasterAlert.showAlert(jqxhr.message);
        });
    };

    //Utils
    $scope.addVenda = function () {
        $scope.getProdutos();
        $scope.getCliente();
        $scope.venda = { cliente: $scope.cliente, vendaProduto: [], vendaParcela: [] };
    };

    $scope.openModalProdutosDisponiveis = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'produtosDisponiveisContent.html',
            size: 'lg',
            controller: 'modalProdutosDisponiveisInstanceController',
            controllerAs: this,
            resolve: {
                dadosModalProdutosDisponiveis: function () {
                    return $scope.produtos;
                }
            }
        });

        modalInstance.result.then(function (produtosSelecionados) {
            var temProduto = false;
            angular.forEach(produtosSelecionados, function (produto, key) {
                temProduto = false;
                angular.forEach($scope.venda.vendaProduto, function (vendaProduto, key) {
                    if (vendaProduto) {
                        if (vendaProduto.produtoId == produto.id) {
                            temProduto = true;
                            vendaProduto.quantidade += produto.quantidadeSelecionada;
                            if (vendaProduto.quantidade > vendaProduto.produto.quantidade) {
                                vendaProduto.quantidade = vendaProduto.produto.quantidade;
                            }
                        }
                    }
                });
                if (!temProduto) {
                    $scope.venda.vendaProduto.push({ produtoId: produto.id, quantidade: produto.quantidadeSelecionada, valorVenda: produto.valorVenda, produto: produto });
                }
            });
        });
    };

    $scope.naoTemProdutos = function () {
        return $scope.venda.vendaProduto.length == 0;
    };

    $scope.openModalParcelasForm = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'parcelasFormContent.html',
            controller: 'modalParcelasFormInstanceController',
            controllerAs: this,
            resolve: {
                dadosModalParcelasForm: { totalAPagar: $scope.totalAPagar(), restanteAPagar: $scope.restanteAPagar()}
            }
        });

        modalInstance.result.then(function (parcelaForm) {
            var totalPagando = 0;
            var ultimaDataPagamento = null;
            for (var i = 0; i < parcelaForm.quantidade; i++) {
                
                if (i == 0) {
                    ultimaDataPagamento = angular.copy(parcelaForm.dataPagamento);
                }
                else {
                    ultimaDataPagamento.setMonth(ultimaDataPagamento.getMonth() + 1);
                }

                totalPagando = (parseFloat(parcelaForm.valor) + parseFloat(totalPagando)).toFixed(2);
                $scope.venda.vendaParcela.push({ tipoPagamento: parcelaForm.tipoPagamento, valor: parcelaForm.valor, dataPagamento: angular.copy(ultimaDataPagamento), feito: parcelaForm.feito });
            }
            $scope.totalPago = parseFloat($scope.totalPago) + parseFloat(totalPagando);
        });
    };

    $scope.addProdutoSelecionado = function (vendaProduto) {
        if (!vendaProduto.quantidade) {
            vendaProduto.quantidade = 1;
        }
        else {
            if (vendaProduto.quantidade < vendaProduto.produto.quantidade) {
                vendaProduto.quantidade++;
            }
        }
    };

    $scope.delProdutoSelecionado = function (vendaProduto) {
        if (vendaProduto.quantidade) {
            if (vendaProduto.quantidade > 1) {
                vendaProduto.quantidade--;
            }
            else {
                vendaProduto.quantidade = 1;
            }
        }
    };

    $scope.delProdutoSelecionadoVenda = function (vendaProduto) {
        $scope.venda.vendaProduto.splice($scope.venda.vendaProduto.indexOf(vendaProduto), 1);
    };

    $scope.delParcela = function (vendaParcela) {
        $scope.totalPago = (parseFloat($scope.totalPago) - parseFloat(vendaParcela.valor)).toFixed(2);
        $scope.venda.vendaParcela.splice($scope.venda.vendaParcela.indexOf(vendaParcela), 1);
    };

    $scope.calculaValorVendaTotal = function (quantidade, valorVenda) {
        return parseFloat(quantidade * valorVenda).toFixed(2);
    };

    $scope.totalAPagar = function () {
        var total = 0;
        angular.forEach($scope.venda.vendaProduto, function (vendaProduto, key) {
            total = (parseFloat(total) + parseFloat($scope.calculaValorVendaTotal(vendaProduto.quantidade, vendaProduto.valorVenda))).toFixed(2);
        });
        return total;
    };

    $scope.restanteAPagar = function () {
        if ($scope.totalPago == 0) {
            return $scope.totalAPagar();
        }
        else {
            return ($scope.totalAPagar() - $scope.totalPago).toFixed(2);
        }

    };
});

app.controller('modalProdutosDisponiveisInstanceController', function ($scope, $uibModalInstance, dadosModalProdutosDisponiveis) {
    $scope.produtos = dadosModalProdutosDisponiveis;
    $scope.produtosSelecionados = [];
    
    angular.forEach($scope.produtos, function (produto, key) {
        produto.quantidadeSelecionada = null;
    });

    $scope.confirmar = function () {

        angular.forEach($scope.produtos, function (produto, key) {
            if (produto.quantidadeSelecionada) {
                $scope.produtosSelecionados.push(produto);
            }
        });

        $uibModalInstance.close($scope.produtosSelecionados);
    };

    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };

    //PAGINATION
    $scope.total = $scope.produtos.length;
    $scope.currentPage = 1;
    $scope.itemPerPage = 5;
    $scope.start = 0;
    $scope.maxSize = 5;
    $scope.pageChanged = function () {
        $scope.start = ($scope.currentPage - 1) * $scope.itemPerPage;
    };

    $('.integer').mask('###0', { reverse: true });

    $scope.addProdutoSelecionado = function (produtoDisponivel) {
        if (!produtoDisponivel.quantidadeSelecionada) {
            produtoDisponivel.quantidadeSelecionada = 1;
        }
        else {
            if (produtoDisponivel.quantidadeSelecionada < produtoDisponivel.quantidade) {
                produtoDisponivel.quantidadeSelecionada++;
            }
        }

    };

    $scope.delProdutoSelecionado = function (produtoDisponivel) {
        if (produtoDisponivel.quantidadeSelecionada) {
            if (produtoDisponivel.quantidadeSelecionada > 1) {
                produtoDisponivel.quantidadeSelecionada--;
            }
            else {
                produtoDisponivel.quantidadeSelecionada = null;
            }
        }

    };
});

app.controller('modalParcelasFormInstanceController', function ($scope, $uibModalInstance, dadosModalParcelasForm) {
    $scope.tiposPagamento = { options: ['Dinheiro', 'Cheque', 'Depósito'], selected: 'Dinheiro' };
    $scope.totalAPagar = parseFloat(dadosModalParcelasForm.totalAPagar).toFixed(2);
    $scope.restanteAPagar = parseFloat(dadosModalParcelasForm.restanteAPagar).toFixed(2);
    $scope.parcelaForm = { quantidade: 1, tipoPagamento: $scope.tiposPagamento.selected, dataPagamento: new Date(), valor: $scope.restanteAPagar };

    $scope.confirmar = function () {
        if ((!$scope.parcelaForm.valor ||
            ($scope.parcelaForm.valor && $scope.parcelaForm.valor == null) ||
            ($scope.parcelaForm.valor && $scope.parcelaForm.valor == '') ||
            ($scope.parcelaForm.valor && $scope.parcelaForm.valor == 0))
            ||
            (!$scope.parcelaForm.dataPagamento ||
            ($scope.parcelaForm.dataPagamento && $scope.parcelaForm.dataPagamento == null) ||
            ($scope.parcelaForm.dataPagamento && $scope.parcelaForm.dataPagamento == null) ||
            ($scope.parcelaForm.dataPagamento && $scope.parcelaForm.dataPagamento.length < 10))
            ) {
            return false;
        }
        $scope.parcelaForm.valor = $('.money').masked($scope.parcelaForm.valor);
        $uibModalInstance.close($scope.parcelaForm);
    };

    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.addParcelaQuantidade = function () {
        $scope.parcelaForm.quantidade++;
        $scope.parcelaForm.valor = ($scope.restanteAPagar / $scope.parcelaForm.quantidade).toFixed(2);
    };
    $scope.delParcelaQuantidade = function () {
        if ($scope.parcelaForm.quantidade > 1) {
            $scope.parcelaForm.quantidade--;
            $scope.parcelaForm.valor = ($scope.restanteAPagar / $scope.parcelaForm.quantidade).toFixed(2);
        }
    };

    $('.money').mask('#0.00', { reverse: true });
    

    //DatePicker
    $scope.openDatePicker = function () {
        $scope.popupDatePicker.opened = true;
    };

    $scope.popupDatePicker = {
        opened: false
    };
});