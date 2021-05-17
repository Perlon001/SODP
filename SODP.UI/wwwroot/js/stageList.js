$(function () {

    var placeholderElement = $('#modal-placeholder');

    $('[data-toggle="tooltip"]').tooltip()

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            console.log(data);
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
            console.log(newBody.find('[name="IsValidate"]'));
            var isValid = newBody.find('[name="IsValidate"]').val() == 'True';
            if (isValid) {
                var stage = document.getElementById("sign").value;
                placeholderElement.find('.modal').modal('hide');
                window.location = 'Stages?tostage='+stage;
            }
        });
    });
});

