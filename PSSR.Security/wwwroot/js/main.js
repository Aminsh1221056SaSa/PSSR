(function ($) {
    $(".toggle-password").click(function() {
        $(this).toggleClass("zmdi-eye zmdi-eye-off");
        var input = $($(this).attr("toggle"));
        if (input.attr("type") == "password") {
          input.attr("type", "text");
        } else {
          input.attr("type", "password");
        }
    });

    $("#password").focusin(function () {
        var input = $(this);
        input.removeAttr('readonly');
    });
})(jQuery);