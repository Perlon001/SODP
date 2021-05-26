function Restore(url) {
    swal({
        title: "Czy jesteś pewien?",
        text: "Przywrócenie projektu do aktywnych.",
        icon: "warning",
        buttons: true,
        danegerMode: true
    }).then((willRestore) => {
        console.log(url);
        if (willRestore) {
            $.ajax({
                type: "POST",
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