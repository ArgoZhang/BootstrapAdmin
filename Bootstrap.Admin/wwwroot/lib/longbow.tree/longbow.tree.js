(function ($) {
    /**
     * Constants
     * ====================================================
     */
    var NAME = 'Treeview';
    var DATA_KEY = 'lte.treeview';
    var EVENT_KEY = "." + DATA_KEY;
    var JQUERY_NO_CONFLICT = $.fn[NAME];
    var Event = {
        SELECTED: "selected" + EVENT_KEY,
        EXPANDED: "expanded" + EVENT_KEY,
        COLLAPSED: "collapsed" + EVENT_KEY,
        LOAD_DATA_API: "load" + EVENT_KEY
    };
    var Selector = {
        LI: '.nav-item',
        LINK: '.nav-link',
        TREEVIEW_MENU: '.nav-treeview',
        OPEN: '.menu-open',
        DATA_WIDGET: '[data-widget="treeview"]'
    };
    var ClassName = {
        LI: 'nav-item',
        LINK: 'nav-link',
        TREEVIEW_MENU: 'nav-treeview',
        OPEN: 'menu-open'
    };
    var Default = {
        trigger: Selector.DATA_WIDGET + " " + Selector.LINK,
        animationSpeed: 300,
        accordion: true
        /**
         * Class Definition
         * ====================================================
         */

    };

    var Treeview =
        /*#__PURE__*/
        function () {
            function Treeview(element, config) {
                this._config = config;
                this._element = element;
            } // Public


            var _proto = Treeview.prototype;

            _proto.init = function init() {
                this._setupListeners();
            };

            _proto.expand = function expand(treeviewMenu, parentLi) {
                var _this = this;

                var expandedEvent = $.Event(Event.EXPANDED);

                if (this._config.accordion) {
                    var openMenuLi = parentLi.siblings(Selector.OPEN).first();
                    var openTreeview = openMenuLi.find(Selector.TREEVIEW_MENU).first();
                    this.collapse(openTreeview, openMenuLi);
                }

                treeviewMenu.slideDown(this._config.animationSpeed, function () {
                    parentLi.addClass(ClassName.OPEN);
                    $(_this._element).trigger(expandedEvent);
                });
            };

            _proto.collapse = function collapse(treeviewMenu, parentLi) {
                var _this2 = this;

                var collapsedEvent = $.Event(Event.COLLAPSED);
                treeviewMenu.slideUp(this._config.animationSpeed, function () {
                    parentLi.removeClass(ClassName.OPEN);
                    $(_this2._element).trigger(collapsedEvent);
                    treeviewMenu.find(Selector.OPEN + " > " + Selector.TREEVIEW_MENU).slideUp();
                    treeviewMenu.find(Selector.OPEN).removeClass(ClassName.OPEN);
                });
            };

            _proto.toggle = function toggle(event) {
                var $relativeTarget = $(event.currentTarget);
                var treeviewMenu = $relativeTarget.next();

                if (!treeviewMenu.is(Selector.TREEVIEW_MENU)) {
                    return;
                }

                event.preventDefault();
                var parentLi = $relativeTarget.parents(Selector.LI).first();
                var isOpen = parentLi.hasClass(ClassName.OPEN);

                if (isOpen) {
                    this.collapse($(treeviewMenu), parentLi);
                } else {
                    this.expand($(treeviewMenu), parentLi);
                }
            } // Private
                ;

            _proto._setupListeners = function _setupListeners() {
                var _this3 = this;

                $(document).on('click', this._config.trigger, function (event) {
                    _this3.toggle(event);
                });
            } // Static
                ;

            Treeview._jQueryInterface = function _jQueryInterface(config) {
                return this.each(function () {
                    var data = $(this).data(DATA_KEY);

                    var _config = $.extend({}, Default, $(this).data());

                    if (!data) {
                        data = new Treeview($(this), _config);
                        $(this).data(DATA_KEY, data);
                    }

                    if (config === 'init') {
                        data[config]();
                    }
                });
            };

            return Treeview;
        }();
    /**
     * Data API
     * ====================================================
     */


    $(window).on(Event.LOAD_DATA_API, function () {
        $(Selector.DATA_WIDGET).each(function () {
            Treeview._jQueryInterface.call($(this), 'init');
        });
    });
    /**
     * jQuery API
     * ====================================================
     */

    $.fn[NAME] = Treeview._jQueryInterface;
    $.fn[NAME].Constructor = Treeview;

    $.fn[NAME].noConflict = function () {
        $.fn[NAME] = JQUERY_NO_CONFLICT;
        return Treeview._jQueryInterface;
    };
})(jQuery);