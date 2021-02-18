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
                        myTable.ajax.reload();                  // refresh only dataTable not whole page :(
                        window.location.reload(true);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}