var table = null;
$(document).ready(function () {
    table = $('#mdastable').DataTable({
            serverSide: true,
            processing: true,
            "searching": true,
            "paging": true,
            "info": true,
            ajax: {
                url: '/view/ministryaccount',
            type: 'POST',
            dataType: 'json'
                        },
            columns: [
            {
                data: null,  orderable: false,  searchable: false, width: '5%',
                render: function (data, type, row, meta) {
                                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            { data: 'name', "autoWidth": true },
            { data: 'code', "autoWidth": true },
            { data: 'description', "autoWidth": true },
            {
                data: null, orderable: false,
                render: function (data, type, row) {
                    if (data.status) {
                        return '<span class="badge bg-success opacity-75"><i class="fas fa-angle-down"></i> Active</span>';
                    } else {
                        return '<span class="badge bg-danger opacity-75"><i class="fas fa-angle-down"></i> Inactive</span>';
                    }
                }
            },
            {
                "data": "createdate", "autoWidth": true,
                render: function (data, type, row) {
                    if (data != null) {
                        var hours = new Date(data).getHours()
                        let ap = hours >= 12 ? 'pm' : 'am';
                        return data = data.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                    } else {
                        return "null";
                    }
                }
            },
            {
                data: null,  orderable: false,
                render: function (data, type, row) {
                    if (data.status) {
                        return `<a class="btn btn-sm btn-outline-secondary rounded-5" data-id="${data.id}" data-bs-toggle="modal" data-bs-target="#managemdasModal" href="#"><i class="ti-settings"></i> Manage</a>`;
                    } else {
                        return ` <a class="btn btn-sm btn-outline-secondary rounded-5" data-id="${data.id}" data-bs-toggle="modal" data-bs-target="#manageposterminalModal" href="#"><i class="ti-reload"></i> Reactivate</a>`;
                    }
                }
            }
            ],

            // Language configuration
            language: {
                processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span>',
            lengthMenu: 'Show _MENU_ entries',
            zeroRecords: 'No matching records found',
            info: 'Showing _START_ to _END_ of _TOTAL_ entries',
            infoEmpty: 'Showing 0 to 0 of 0 entries',
            infoFiltered: '(filtered from _MAX_ total entries)',
            search: 'Search:',
            paginate: {
                first: 'First',
            last: 'Last',
            next: 'Next',
            previous: 'Prev'
                            }
                        },

            // Custom initialization
            initComplete: function () {
                // Add any initialization code here
                console.log('Table initialized');
                        }
                    });
                });

            $(document).on('click', '.delete-btn', function () {
                    const id = $(this).data('id');
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
                        },
            buttonsStyling: false
                    })

            swalWithBootstrapButtons.fire({
                title: 'Are you sure?',
            text: "You won't be able to recover this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
                    }).then((result) => {
                        if (result.isConfirmed) {
                $.ajax({
                    type: "DELETE",
                    url: '/v1/deletelga/' + id,
                    success: function (data) {
                        $.unblockUI();
                        if (data.success) {
                            swalWithBootstrapButtons.fire(
                                'Deleted!',
                                data.message,
                                'success'
                            );
                            table.ajax.reload(null, false);
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: data.message,
                                footer: 'Message'
                            });
                        }
                    },
                    beforeSend: function () {
                        blockcallback();
                    },
                    error: function () {
                        $.unblockUI();
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong!',
                            footer: 'Check internet connectivity'
                        });
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
                        } else if (result.dismiss === Swal.DismissReason.cancel) {
                swalWithBootstrapButtons.fire(
                    'Cancelled',
                    'LGA NOT Deleted',
                    'error'
                );
                }
            });

        });
    function blockcallback() {
        $.blockUI({
            message: '<div class="bs-spinner mt-4 mt-lg-0"><div class= "spinner-border text-success mr-2 mt-2" style="width: 10rem; height: 10rem;" role="status"><span class="sr-only">Loading...</span></div> ',
            fadeIn: 800,
            overlayCSS: {
                backgroundColor: '#1b2024',
                opacity: 0.8,
                zIndex: 1200,
                cursor: 'wait'
            },
            css: {
                border: 0,
                color: '#fff',
                zIndex: 1201,
                padding: 0,
                backgroundColor: 'transparent'
            },
            onBlock: function () {

            }
        });
   }