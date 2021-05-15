function Delete(url) {
    swal({
        title: "Czy jesteś pewien?",
        text: "Usunięcie projektu. Operacja nie może być cofnięta.",
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
                        window.location.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}