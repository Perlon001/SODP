$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip()
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            var placeholderElement = $('#modal-placeholder');
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    InitProjectDataTable();
    InitModalPlaceHolder();
});

function InitProjectDataTable() {
    $('#ProjectDataTable').DataTable({
        data: ProjectsArray,
        columns: [
            {
                data: "number",
                render: function (data, type, row) {
                    return `<a onclick="OpenProjectEdit(${row.id})" data-toggle="tooltip" title="Edycja nazwy" cursor="">${row.number}</a>`;
                }
            },
            {
                data: "stageSign",
                render: function (data, type, row) {
                    return `<a onclick="OpenProjectEdit(${row.id})" data-toggle="tooltip" title="Edycja nazwy">${row.stageSign}</a>`;
                }
            },
            {
                data: "title",
                render: function (data, type, row) {
                    return `<a onclick="OpenProjectEdit(${row.id})" data-toggle="tooltip" title="Edycja nazwy">${row.title}</a>`;
                }
            },
            {
                data: "id",
                render: function (data) {
                    var renderTags = `<div class="row group justify-content-center">`;
                    renderTags += `<a onclick = 'Delete("/api/ActiveProjects/${data}")' class='btn btn-sm btn-danger text-white mb-0 mt-0 ml-1 mr-1 p-1' style = "cursor:pointer; width=70px;" data-toggle="tooltip" data-placement="top" title="Usuń projekt"> <i class="far fa-trash-alt"></i></a >`;
                    renderTags += `<a onclick = 'Archive("/api/ActiveProjects/${data}")' class='btn btn-sm btn-success text-white mb-0 mt-0 ml-1 mr-1 p-1' style = "cursor:pointer; width=70px;" data-toggle="tooltip" data-placement="top" title="Przenieś projekt do archiwum"> <i class="fas fa-archive"></i></a >`;
                    renderTags += `</div >`;

                    return renderTags
                },
                width: "15%",
                visible: RoleCheck
            }
        ]
    });
}

function InitModalPlaceHolder() {
    var placeholderElement = $('#modal-placeholder');

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();
        $.post(actionUrl, dataToSend).done(function (data) {
            console.log(data);
            var newBody = $('.modal-body', data);
            console.log(newBody);
            placeholderElement.find('.modal-body').replaceWith(newBody);
            var isValid = newBody.find('[name="IsValidate"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
                window.location.reload();
            }
        });
    });
}
