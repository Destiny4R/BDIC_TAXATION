var table = null;
$(document).ready(function () {
    table = $('#assessmentTable').DataTable({
        serverSide: true,
        processing: true,
        "searching": true,
        "paging": true,
        "info": true,
        responsive: true,
        ajax: {
            url: '/view/TaxAssessmentViews',
            type: 'POST',
            dataType: 'json'
        },

        // Column definitions
        columns: [
            {
                data: null,
                orderable: false,
                searchable: false,
                autoWidth: true,
                render: function (data, type, row, meta) {
                    return meta.settings._iDisplayStart + meta.row + 1;
                }
            },
            { data: 'companyname', "autoWidth": true },
            { data: 'username', "autoWidth": true },
            { data: 'session', "autoWidth": true },
            {
                "data": "session", "autoWidth": true,
                render: function (data, type, row) {
                    let num = Number(data);
                    return ((num+1)+', Dec');
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
            { data: 'income', "autoWidth": true },
            { data: 'taxamount', "autoWidth": true },
            {
                "data": "status", "autoWidth": true,
                render: function (data, type, row) {
                    if (data) {
                        return '<span class="badge badge-success"><i class="far fa-thumbs-up"></i></span>';
                    } else {
                        return '<span class="badge badge-warning"><i class="far fa-thumbs-down"></i></span>';
                    }
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    let btntext = 'Reject';
                    let iconclass = 'mdi-close-circle-outline';
                    if (!data.status) {
                        btntext = "Approve";
                        iconclass = 'mdi-check-underline-circle-outline';
                    }
                    //Create a val for checking status
                    // Create the main button group container
                    var $btnGroup = $('<div>').addClass('btn-group');

                    // Create the dropdown toggle button
                    var $toggleBtn = $('<button>')
                        .attr('type', 'button')
                        .addClass('btn btn-light')
                        .attr('data-bs-toggle', 'dropdown')
                        .append($('<i>').addClass('mdi mdi-dots-horizontal'));

                    // Create the dropdown menu
                    var $dropdownMenu = $('<ul>').addClass('dropdown-menu');

                    // Menu items configuration
                    var menuItems = [
                        {
                            icon: 'mdi-eye-circle-outline',
                            text: 'View',
                            className: 'btn-view',
                        },
                        {
                            icon: iconclass,
                            id: "theactiontag",
                            text: btntext,
                            className: _getbtnproperty(data.status), //'btn-reject' Default class
                            toggleClass: 'btn-download' // Class to toggle with
                        },
                        {
                            icon: 'mdi-flag-outline',
                            text: 'Flag',
                            className: 'btn-reject'
                        }
                    ];


                    // Add menu items
                    $.each(menuItems, function (i, item) {
                        var $link = $('<a>')
                            .addClass('dropdown-item ' + item.className)
                            .attr('href', '#')
                            .attr('data-id', data.id);

                        // Add ID if it exists in the item configuration
                        if (item.id) {
                            $link.attr('id', item.id);
                        }

                        $link.append(
                            $('<i>').addClass('mdi ' + item.icon),
                            document.createTextNode(' ' + item.text)
                        );

                        if (item.attrs) {
                            $link.attr(item.attrs);
                        }
                    $('#theactiontag').toggleClass(`${item.className} ${item.toggleClass}`);

                        $dropdownMenu.append($('<li>').append($link));
                    });
                    // Assemble the components
                    $btnGroup.append($toggleBtn).append($dropdownMenu);

                    // Return the HTML string (DataTables expects this)
                    return $btnGroup[0].outerHTML;
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
                first: '<i class="fas fa-angle-left"></i>',
                last: '<i class="fas fa-angle-right"></i>',
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
// Create a function to return string from a bollean property
function _getbtnproperty(bool) {
    if (!bool) {
        return "btn-approve";
    } else {
        return "btn-reject";
    }
}
// Event delegation for button clicks for rejecting a request
$(document).on('click', '.btn-approve', function () {
   const id = $(this).data('id');
    rejectAndApprove(id);
});
function rejectAndApprove(id) {
    
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You want to reject this request!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: '/v1/RejectTaxRequest',
                data: {
                    id: id,
                    isoption: true
                },
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Information!',
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
                'Action aborted',
                'error'
            );
        }
    });
}
$(document).on('click', '.btn-view', function () {
    const id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/v1/GetTaxAssessmentedata/' + id,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                var dx = data.data;
                $('#name').text(dx.companyname);
                $('#assetname').text(dx.username);
                $('#session').text(dx.session);
                $('#income').text(dx.income);
                $('#phone').text(dx.phone);
                $('#taxamount').text(dx.taxamount);
                $('#btn-download').text(dx.filepath);
                $('#leftamount').text(dx.amountleft);

                const dateObj = new Date(dx.createdate);
                const options = {
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: true
                };
                const formattedDate = dateObj.toLocaleString('en-US', options);
                $('#createdate').text(formattedDate);
                //var hours = new Date(data.data.createdate).getHours()
                //let ap = hours >= 12 ? 'pm' : 'am';
                //let date = data.data.createddate.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                //$('#createdate').text(date);

                //Working below
                $('.btn-download').attr('data-id', dx.id);
                var $btnApprove = $('.btn-approve');
                $btnApprove.empty(); // Clear existing content

                var $icon = $('<i>').addClass('mdi');
                var $text = $('<span>');

                if (!dx.status) {
                    $btnApprove.addClass('btn-success').attr('data-id', dx.id);
                    $icon.addClass('mdi-check-underline-circle-outline');
                    $text.text(' Approve');
                } else {
                    $btnApprove.addClass('btn-danger').attr('data-id', dx.id);;
                    $icon.addClass('mdi-close-circle-outline');
                    $text.text(' Reject');
                }

                $btnApprove.append($icon, $text);
                //$('.btn-enable-Disable').attr('data-id', data.data.id);
                // Set document name text (safe against XSS)
                //$('.docu-name').text(dx.filename); 
                $('#docu-name').text(typeof dx.filename === 'string' ? dx.filename.trim() : '');
                const $downloadContainer = $('#div-download').empty();
                if (dx.filepath) {
                    // Create a more robust download link
                    const downloadLink = $('<a>', {
                        href: encodeURI(dx.filepath.startsWith('/') ? dx.filepath : '/' + dx.filepath),
                        target: '_blank',
                        class: 'btn btn-sm btn-light w-100 btn-download',
                        title: 'Download document',
                        'aria-label': 'Download document' // Better accessibility
                    }).append(
                        $('<i>', {
                            class: 'dripicons-download mr-1' // Added margin for spacing
                        })// Added text fallback
                    );

                    $downloadContainer.html(downloadLink);
                } else {
                    $downloadContainer.html('<span class="text-muted">No document available</span>');
                }
                const $statusDiv = $('#divmessage').empty(); // Clear previous content
                //<span class="badge bg-success">Filed</span>
                if (dx.status) {
                    $statusDiv.append('<strong>Status: </strong> <span class="badge badge-success"><i class="fas fa-check-double"></i></span>');
                } else {
                    $statusDiv.append('<strong>Status: </strong> <span class="badge badge-danger"><i class="fas fa-times"></i></span>');
                }
                                
                const view = new bootstrap.Modal('#assessmentDetailsModal');
                view.show();

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
});

$(document).on('click', '.btn-reject', function () {
    const id = $(this).data('id');
    Swal.fire({
        title: "Submit Rejection Message",
        html: `
        <div class="form-group text-left">
            <label for="statusSelect" class="d-block text-left">Select Status:</label>
            <select id="statusSelect" class="custom-select">
                <option value="" selected disabled>-- Select an option --</option>
                <option value="Flagged by legal">Flagged by legal</option>
                <option value="Active">Active</option>
                <option value="Closed">Closed</option>
            </select>
        </div>
        <div class="form-group text-left">
            <label for="rejectReason" class="d-block text-left">Enter reason for rejection:</label>
            <textarea id="rejectReason" class="form-control" rows="5" placeholder="Enter reason for rejection..." maxlength="500"></textarea>
        </div>
    `,
        preConfirm: async () => {
            const msg = document.getElementById("rejectReason").value;
            const selectOption = document.getElementById("statusSelect").value;

            if (!selectOption) {
                Swal.showValidationMessage("Please select an option");
                return false;
            }
            if (!msg) {
                Swal.showValidationMessage("Message cannot be empty");
                return false;
            }

            try {
                const Url = `/v1/RejectTaxRequest?id=${id}&reason=${msg}&status=${selectOption}`;
                $.ajax({
                    type: "POST",
                    url: Url,
                    data: {
                        id: id,
                        reason: msg,
                        status: selectOption,
                        isoption: false
                    },
                    success: function (data) {
                        $.unblockUI();
                        Swal.fire({
                            title: "Reason Submitted",
                            text: data.message,
                            icon: data.success ? "success" : "error"
                        });
                    },
                    error: function () {
                        $.unblockUI();
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "Something went wrong!",
                            footer: "Check internet connectivity"
                        });
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
            } catch (error) {
                Swal.showValidationMessage(`Request failed: ${error}`);
                return false;
            }
        },
        showCancelButton: true,
        confirmButtonText: "Submit",
        showLoaderOnConfirm: true,
        allowOutsideClick: () => !Swal.isLoading()
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