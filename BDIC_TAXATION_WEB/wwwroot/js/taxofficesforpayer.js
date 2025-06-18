var table = null;
$(document).ready(function () {
    table = $('#taxofficesTable').DataTable({
        serverSide: true,
        processing: true,
        "searching": true,
        "paging": true,
        "info": true,
        "responsive": true,
        ajax: {
            url: '/view/TaxOfficesViews',
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
            { data: 'address', "autoWidth": true },
            { data: 'phone', "autoWidth": true },
            { data: 'lga', "autoWidth": true },
            { data: 'email', "autoWidth": true },
            {
                "data": "status", "autoWidth": true,
                render: function (data, type, row) {
                    if (data) {
                        return '<span class="badge badge-success"><i class="fas fa-chevron-down"></i> Active</span>';
                    } else {
                        return '<span class="badge badge-warning"><i class="fas fa-chevron-down"></i> Inactive</span>';
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

