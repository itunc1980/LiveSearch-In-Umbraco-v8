(function () {
    'use strict';

    function langController($scope, uSyncActionManager) {
        var pvm = this;

        pvm.loading = true;
        pvm.all = true;

        console.log($scope.vm.server.name);

        pvm.process = $scope.vm.process;
        pvm.mode = $scope.vm.mode;
        pvm.flags = $scope.vm.flags;
        pvm.server = $scope.vm.server;

        pvm.showChildren = $scope.vm.items.length > 1 || $scope.vm.items[0].hasChildren;
        pvm.deleteMissingInitalValue = pvm.flags.deleteMissing.value;

        pvm.itemtype = 'content';

        pvm.fulllang = false;
        pvm.hasUnpublished = false;
        pvm.selectAll = selectAll;

        pvm.$onInit = function () {
            setTitle();
            getVariants();
            pvm.loading = false;
        }

        function selectAll() {
            _.forEach(pvm.variants, function (variant) {
                variant._checked = (pvm.fulllang || variant.published);
            });
        }

        function setTitle() {
            if ($scope.model != null) {
                $scope.model.title = 'Select languages';
                $scope.model.subtitle = "Select Languages to publish to " + pvm.server.name;
            }
        }

        function getVariants() {
            pvm.variants = _.map($scope.vm.items[0].allVariants, function (info, id) {

                if (!info.published) {
                    pvm.hasUnpublished = true;
                }

                return {
                    _checked: info.published,
                    name: info.name,
                    id: id,
                    published: info.published,
                    _state: info.published ? '' : '(not published locally)'
                }
            });
        }

        var watchEvent = $scope.$watch('pvm.variants', function (newValue) {
            if (newValue !== undefined) {
                pvm.process.options.cultures = [];
                $scope.vm.state.valid = false;

                for (let i = 0; i < newValue.length; i++) {
                    if (newValue[i]._checked === true) {
                        pvm.process.options.cultures.push(newValue[i].id);
                        $scope.vm.state.valid = true;
                    }
                }

                if (pvm.process.options.cultures.length == newValue.length) {
                    // all selected
                    pvm.process.options.cultures = []
                    pvm.all = true;
                    pvm.flags.deleteMissing.value = pvm.deleteMissingInitalValue;
                }
                else {
                    // 
                    pvm.flags.deleteMissing.value = false;
                    pvm.all = false;
                }

                pvm.process.options.removeOrphans = pvm.flags.deleteMissing.value;

            }
        }, true);

        $scope.$on('$destroy', function () {
            watchEvent();
        })
    }

    angular.module('umbraco')
        .controller('uSyncPublisherLanguageController', langController);

})();