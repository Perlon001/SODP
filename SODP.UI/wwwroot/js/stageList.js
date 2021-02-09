var myTable;
$(document).ready(function () {
    $(document).ready(function () {
        myTable = $('#StageDataTable').DataTable({
            ajax: {
                url: "/api/Stages",
                type: "GET",
                dataSrc: 'data.collection',
            },
            columns: [
                { data: "sign" },
                { data: "description" },
                {
                    data: "sign",
                    render: function (data) {
                        return `<div class="text-center">
                                <a href='/Stages/Edit?sign=${data}' class='btn btn-sm btn-info text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;">Edytuj</a>
                                <a onclick='Delete("/api/Stages/${data}")' class='btn btn-sm btn-danger text-white mb-0 mt-0 ml-1 mr-1 p-1' style="cursor:pointer; width=70px;">Usuń</a>
                            </div>`;
                    },
                    width: "15%"
                }
            ]
        });
    });
});


function Delete(url) {
    swal({
        title: "Czy jesteś pewien?",
        text: "Operacja nie może być cofnięta.",
        icon: "warning",
        buttons: true,
        danegerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        myTable.ajax.reload();                  // refresh only dataTable not whole page
                        reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}

function reload() {
    var container = document.getElementById("StageTable");
    var content = container.innerHTML;
    container.innerHTML = content;

    //this line is to watch the result in console , you can remove it later	
    window.location.replace("/Stages");
}