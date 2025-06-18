$(document).ready(function () {
    var $form = $('form.is-invalid');
    var $tabs = $form.find('.nav-link');
    var $tabContent = $form.find('.tab-pane');
    var currentTab = 0;

    // Function to show a specific tab
    function showTab(index) {
        $tabs.removeClass('active');
        $tabContent.removeClass('show active');
        $tabs.eq(index).addClass('active');
        $tabContent.eq(index).addClass('show active');
        currentTab = index;
    }

    // Function to validate the current tab
    function validateTab(index) {
        var isValid = true;
        var $currentTabContent = $tabContent.eq(index);
        $currentTabContent.find('.required').each(function () {
            if ($(this).val() === '') {
                isValid = false;
                $(this).addClass('is-invalid'); // Add Bootstrap's invalid class
            } else {
                $(this).removeClass('is-invalid');
            }
        });
        return isValid;
    }

    // Event listener for "Next" button click (assuming tab buttons act as next)
    $tabs.on('click', function (e) {
        e.preventDefault();
        var targetIndex = $(this).index();

        // Only move to the clicked tab if the current tab is valid or if clicking on a previous tab
        if (targetIndex > currentTab) {
            if (validateTab(currentTab)) {
                showTab(targetIndex);
            }
        } else {
            showTab(targetIndex);
        }
    });

    // Event listener for form submission
    $form.on('submit', function (e) {
        var allTabsValid = true;
        $tabs.each(function (index) {
            if (!validateTab(index)) {
                allTabsValid = false;
            }
        });

        if (!allTabsValid) {
            e.preventDefault(); // Prevent form submission if any tab is invalid
            // Optionally, you can navigate the user to the first invalid tab
            $tabs.each(function (index) {
                if (!validateTab(index)) {
                    showTab(index);
                    return false; // Break the loop
                }
            });
        }
        // If all tabs are valid, the form will submit normally
    });
});