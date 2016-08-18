var app = angular.module('app', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'ngCookies', 'toaster'])
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider.when('/', { templateUrl: 'app/templates/home.html', controller: 'homeController' });
        $routeProvider.when('/signin', { templateUrl: 'app/templates/signin.html', controller: 'signinController' });
        $routeProvider.when('/perfil', { templateUrl: 'app/templates/perfil/perfis.html', controller: 'perfilController' });
        $routeProvider.when('/perfil/add', { templateUrl: 'app/templates/perfil/perfil-add.html', controller: 'perfilController' });
        $routeProvider.when('/perfil/:id/edit', { templateUrl: 'app/templates/perfil/perfil-edit.html', controller: 'perfilController' });
        $routeProvider.when('/perfil/:id', { templateUrl: 'app/templates/perfil/perfil-view.html', controller: 'perfilController' });
        $routeProvider.when('/usuario', { templateUrl: 'app/templates/usuario/usuarios.html', controller: 'usuarioController' });
        $routeProvider.when('/usuario/add', { templateUrl: 'app/templates/usuario/usuario-add.html', controller: 'usuarioController' });
        $routeProvider.when('/usuario/:id/edit', { templateUrl: 'app/templates/usuario/usuario-edit.html', controller: 'usuarioController' });
        $routeProvider.when('/usuario/:id', { templateUrl: 'app/templates/usuario/usuario-view.html', controller: 'usuarioController' });
        $routeProvider.when('/alterarsenha', { templateUrl: 'app/templates/usuario/usuario-alterarSenha.html', controller: 'alterarSenhaController' });
        $routeProvider.when('/esquecisenha', { templateUrl: 'app/templates/usuario/usuario-esqueciSenha.html', controller: 'esqueciSenhaController' });
        $routeProvider.when('/log', { templateUrl: 'app/templates/logs/logs.html', controller: 'logController' });
        $routeProvider.when('/paginanaoencontrada', { templateUrl: 'app/templates/paginaNaoEncontrada.html', controller: 'paginaNaoEncontradaController' });

        $routeProvider.when('/produto', { templateUrl: 'app/templates/produto/produtos.html', controller: 'produtoController' });
        $routeProvider.when('/produto/add', { templateUrl: 'app/templates/produto/produto-add.html', controller: 'produtoController' });
        $routeProvider.when('/produto/:id/edit', { templateUrl: 'app/templates/produto/produto-edit.html', controller: 'produtoController' });
        $routeProvider.when('/produto/:id', { templateUrl: 'app/templates/produto/produto-view.html', controller: 'produtoController' });

        $routeProvider.when('/cliente', { templateUrl: 'app/templates/cliente/clientes.html', controller: 'clienteController' });
        $routeProvider.when('/cliente/add', { templateUrl: 'app/templates/cliente/cliente-add.html', controller: 'clienteController' });
        $routeProvider.when('/cliente/:id/edit', { templateUrl: 'app/templates/cliente/cliente-edit.html', controller: 'clienteController' });
        $routeProvider.when('/cliente/:id', { templateUrl: 'app/templates/cliente/cliente-view.html', controller: 'clienteController' });

        $routeProvider.otherwise({ redirectTo: '/paginanaoencontrada' });
        $locationProvider.html5Mode(true);
    });

app.run(function ($rootScope) {
    /*
        Receive emitted message and broadcast it.
        Event names must be distinct or browser will blow up!
    */
    $rootScope.$on('atualizaHeaderEmit', function (event, args) {
        $rootScope.$broadcast('atualizaHeaderBroadcast', args);
    });
});

app.factory('UserService', function ($http, $window, $cookies, $location, toasterAlert) {
    return {
        getUser: function () {
            var user = $cookies.get('user');
            if (user) {
                return JSON.parse(user);
            }
            else {
                return null;
            }
        },
        setUser: function (newUser) {
            if (newUser) {
                var urlFuncionalidade = 'api/funcionalidade/GetForMenu';

                var headerAuth = { headers: { 'Authorization': 'Basic ' + newUser.token } };

                $http.get(urlFuncionalidade, headerAuth).success(function (data) {
                    newUser.menus = data;
                    $cookies.put('user', JSON.stringify(newUser));
                    $location.path('');
                }).error(function (jqxhr, textStatus) {
                    toasterAlert.showAlert(jqxhr.message);
                });
            }
            else {
                $cookies.put('user', null);
            }
        },
        verificaLogin: function () {
            var user = this.getUser();
            if (!user) {
               $location.path('signin');
            }
        }
    };
});

function findAndReplace(string, target, replacement) {
    var i = 0, length = string.length;

    for (i; i < length; i++) {
        string = string.replace(target, replacement);
    }
    return string;
}