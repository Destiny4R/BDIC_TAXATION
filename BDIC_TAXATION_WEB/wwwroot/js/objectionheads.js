var table = null;
$(document).ready(function () {
    table = $('#objectionTable').DataTable({
        serverSide: true,
        processing: true,
        "searching": true,
        "paging": true,
        "info": true,
        "responsive": true,
        ajax: {
            url: '/view/taxObjectionsheads',
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
            { data: 'name', "autoWidth": true },
            { data: 'reference', "autoWidth": true },
            { data: 'payerid', "autoWidth": true },
            { data: 'session', "autoWidth": true },
            { data: 'taxamount', "autoWidth": true },
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
                "data": "objectiontype", "autoWidth": true,
                render: function (data, type, row) {
                    switch (data) {
                        case "0":
                            return '<span class="badge badge-warning">Under review</span>';
                        case "1":
                            return '<span class="badge bg-info"><i class="fas fa - spinner"></i> In Progress</span>';
                        case "2":
                            return '<span class="badge bg-success badge-status"><i class="fas fa-check-double"></i> Resolved</span>';
                        case "3":
                            return '<span class="badge bg-danger opacity-75"><i class="far fa-times-circle"></i> Closed</span>';
                        default:
                            return '<span class="badge bg-dark">N/A</span>';
                    }
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    // Create dropdown container
                    const dropdown = document.createElement('div');
                    dropdown.className = 'dropdown';

                    // Create toggle button
                    const toggleBtn = document.createElement('button');
                    toggleBtn.type = 'button';
                    toggleBtn.className = 'btn btn-sm btn-light ';
                    toggleBtn.setAttribute('data-bs-toggle', 'dropdown');
                    toggleBtn.setAttribute('aria-expanded', 'false');

                    // Add icon to button
                    const icon = document.createElement('i');
                    icon.className = 'mdi mdi-dots-horizontal';
                    toggleBtn.appendChild(icon);

                    // Create dropdown menu
                    const dropdownMenu = document.createElement('ul');
                    dropdownMenu.className = 'dropdown-menu';

                    // Create menu items
                    const menuItems = [
                        {
                            text: 'View',
                            icon: 'mdi-eye-circle-outline',
                            className: 'btn-outline-secondary viewdetail-btn',
                            attributes: {
                                'data-id': row.id,
                                'href': '#'
                            }
                        },
                        {
                            text: 'Resolve',
                            className: 'btn-outline-success btn-resolved-obj',
                            attributes: {
                                'data-id': row.id,
                                'href': '#'
                            }
                        },
                        {
                            text: 'Reject',
                            className: 'btn-outline-danger btn-reject-obj',
                            attributes: {
                                'data-id': row.id,
                                'href': '#',
                            }
                        }
                    ];

                    // Add menu items to dropdown
                    menuItems.forEach(item => {
                        const li = document.createElement('li');
                        const a = document.createElement('a');
                        a.className = `dropdown-item btn btn-sm rounded ${item.className}`;

                        // Set attributes
                        Object.entries(item.attributes).forEach(([key, value]) => {
                            a.setAttribute(key, value);
                        });

                        // Add icon if specified
                        if (item.icon) {
                            const itemIcon = document.createElement('i');
                            itemIcon.className = `mdi ${item.icon}`;
                            a.appendChild(itemIcon);
                            a.appendChild(document.createTextNode(' ' + item.text));
                        } else {
                            a.textContent = item.text;
                        }

                        li.appendChild(a);
                        dropdownMenu.appendChild(li);
                    });

                    // Assemble dropdown
                    dropdown.appendChild(toggleBtn);
                    dropdown.appendChild(dropdownMenu);

                    // Return the outer HTML of the dropdown element
                    return dropdown.outerHTML;
                }
            }
        ],

        // Language configuration
        language: {
            processing: '<div class="spinner-border text-success" style="width: 8rem; height: 8rem;" role="status"><span class="visually-hidden">Loading...</span></div>',
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

$(document).on('click', '.viewdetail-btn', function () {
    const id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/v1/GetObjectionDetailsIndividual/' + id,
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                var dx = data.data;
                $('#income').text(dx.income);
                $('#name').text(dx.name);
                $('#session').text(dx.session);
                $('#payerid').text(dx.payerid);
                $('#reference').text(dx.reference);
                $('#objectiontype').text(dx.objectiontype);
                $('#message').text(dx.message);
                $('#taxamount').text(dx.taxamount);
                $('#status').text(dx.status);
                //$('#createdate').text(date);
                $('#taxamount').text(dx.id);
                var hours = new Date(dx.createddate).getHours()
                let ap = hours >= 12 ? 'pm' : 'am';
                let date = dx.createdate.toLocaleString('YYYY-MM-dd').slice(0, 19).replace('T', ' ') + ' ' + ap;
                //LABEL ICON
                const $labelcon = $('#labeliconDiv').empty();
                $labelcon.append(getSwitchLabel(dx.objectiontype));

                //DOWNLOAD CONTAIN
                let containerdata = `<div class="col-8">
                                <i class="bi bi-file-earmark-pdf fs-1 text-danger"></i>
                                <p class="small mb-1">${dx.filename}</p>
                            </div>
                            <a class="btn btn-sm btn-light col-4" target="_blank" href="${dx.filepath}"><i class="dripicons-download"></i></a>`;
                const containDiv = $('#downloadContainer').empty();
                containDiv.append(containerdata);

                //ADD BUTTONS IN THE DIV 
                const btnDiv = document.getElementById('btn-Div');
                $('#btn-Div').empty();
                // Create the "Resolve" button
                const resolveButton = document.createElement('button');
                resolveButton.type = 'button';
                resolveButton.dataset.id = dx.id; 
                resolveButton.className = 'btn btn-success w-100 btn-resolve';
                resolveButton.textContent = 'Resolve';
                // Create the "Reject Observation" button
                const rejectButton = document.createElement('button');
                rejectButton.type = 'button';
                rejectButton.dataset.id = dx.id; // or set a dynamic ID if needed
                rejectButton.className = 'btn btn-outline-danger w-100 btn-reject';
                rejectButton.textContent = 'Reject Observation';
                // Append both buttons to the div
                btnDiv.appendChild(resolveButton);
                btnDiv.appendChild(rejectButton);

                //$('#status').text(data.data.status);
                const view = new bootstrap.Modal('#objectionDetailModal');
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

$(document).on('click', '.btn-resolved-obj', function () {
    const id = $(this).data('id');
    $('#resoiveid').val(id);
    const view = new bootstrap.Modal('#resolveremarkModal');
    view.show();
});
//REJECT MODAL
$(document).on('click', '.btn-reject-obj', function () {
    const id = $(this).data('id');
    $('#resoiveid').val(id);
    const view = new bootstrap.Modal('#rejectremarkModal');
    view.show();
});

//RESOLVE BTN ACTION
$(document).on('click', '.btn-submitresolve', function () {
    let id = $('#resoiveid').val();
    let message = $('#remark').val();
    sendMessageAction(id, message);
});

//REJECT BTN ACTION
$(document).on('click', '.btn-rejectaction', function () {
    let id = $('#resoiveid').val();
    let message = $('#disabledTextInput').val();
    sendMessageAction(id, message);
});
function sendMessageAction(id, message) {
    if (id === "" || message === "" || message === null || message === " ") {
        return Swal.fire({
            icon: 'error',
            text: 'Remark is required!',
            footer: 'Validation'
        });
    }
    $.ajax({
        type: "POST",
        url: '/v1/ResolvedObjectionAdmin',
        data: {
            id: id,
            remark: message
        },
        success: function (data) {
            $.unblockUI();
            if (data.success) {
                swal.fire(
                    'Action Result!',
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
}
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
//ACTIONS
function getSwitchLabel(data) {
    switch (data) {
        case "0":
            return '<span class="badge badge-warning">Under review</span>';
        case "1":
            return '<span class="badge bg-info">In Progress</span>';
        case "2":
            return '<span class="badge bg-success">Resolved</span>';
        case "3":
            return '<span class="badge bg-danger opacity-75">Closed</span>';
        default:
            return '<span class="badge bg-dark">N/A</span>';
    }
}

$(document).on('click', '.btn-close', function () {
    setTimeout(() => { // Small delay to ensure cleanup
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        view.show();
    }, 500);
});

