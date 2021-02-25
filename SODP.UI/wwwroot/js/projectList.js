﻿var myTable;
$(document).ready(function () {
    myTable = $('#ProjectDataTable').DataTable({
        //ajax: {
        //    url: "/api/Projects",
        //    type: "GET",
        //    dataSrc: 'data.collection',
        //},
        data: ProjectsArray,
        columns: [
            { data: "number" },
            { data: "stage.sign" },
            {
                data: "title",
                render: function (data, type, row) {
                    return `<a href="/Projects/CreateUpdate?id=${row.id}">${row.title}</a>`;
                }
            },
            {
                data: "id",
                render: function (data) {
                    return RenderButton(data);
                },
                width: "15%",
                visible: RoleCheck
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                text: 'Schowek'
            },
            {
                extend: 'excel',
                text: 'Excel',
                className: 'excelButton'
            },
            {
                extend: 'pdf',
                text: 'Pdf'
            }
        ]
    });
});

function RenderButton(data) {
    var renderTags = `<div class="row group justify-content-center">`;
    renderTags += `<a href='/Projects/CreateUpdate?id=${data}' class='btn btn-sm btn-info text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;"><i class="far fa-edit"></i></a>`;
    renderTags += `<a onclick = 'Delete("/api/Projects/${data}")' class='btn btn-sm btn-danger text-white mb-0 mt-0 ml-1 mr-1 p-1' style = "cursor:pointer; width=70px;" > <i class="far fa-trash-alt"></i></a >`;
    renderTags += `</div >`;
    renderTags += `<div id="deleteProjectModal" class="modal fade">`;
    renderTags += `<div class="modal-dialog">`;
    renderTags += `<form>`;
    renderTags += `<div class="modal-footer">`;
    renderTags += `<input type="button" class="btn btn-default" data-dismiss="modal" value="Cancel">`;
    renderTags += `<input type="submit" class="btn btn-danger" value="Delete">`;
    renderTags += `</div>`;
    renderTags += `</form>`;
    renderTags += `</div>`;
    renderTags += `</div>`;

    return renderTags
}

function Reload() {
    var container = document.getElementById("ProjectTable");
    var content = container.innerHTML;
    container.innerHTML = content;

    // this line is to watch the result in console , you can remove it later	
    // window.location.replace("/Projects");
}
