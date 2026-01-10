(function ($) {
    $.fn.convertevalor = function () {
        return $(this).val().replace('R$', '').replace('.', '').replace(',', '.');
    }


})(jQuery);