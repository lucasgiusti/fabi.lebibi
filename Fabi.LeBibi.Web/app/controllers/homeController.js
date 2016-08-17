app.controller('homeController', function ($scope, $window, $location, UserService) {
    UserService.verificaLogin();

    $scope.heading = "Fabi.LeBibi";
    $scope.message = "Sistema template desenvolvido em ASP.Net WebAPI, Angularjs e Bootstrap";
});