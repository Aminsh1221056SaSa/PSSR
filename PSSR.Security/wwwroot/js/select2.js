(function($) {
    'use strict';

    $('.select2').select2();

  if ($(".js-example-basic-single").length) {
    $(".js-example-basic-single").select2();
  }
    if ($(".js-example-basic-IdentityResources").length) {
        $(".js-example-basic-IdentityResources").select2({
            tags: true,
            createTag: function (params) {
                return {
                    id: params.term,
                    text: params.term,
                    newOption: true
                }
            }
        });
    }

})(jQuery);