// common.js
function rebindComparisonTableFilters() {
    $(document).on('change', '.vendor-filter, .food-filter', function () {
        filterComparisonTable();
    });
    filterComparisonTable(); // Ensure the table is filtered correctly
}

$(document).ready(function () {
    // Function to handle filter changes
    function filterComparisonTable() {
        var selectedVendors = [];
        var selectedFoodItems = [];

        // Get selected vendors
        $('.vendor-filter:checked').each(function () {
            var vendorId = $(this).val();
            if (vendorId !== 'all') {
                selectedVendors.push(vendorId);
            }
        });

        // Get selected food items
        $('.food-filter:checked').each(function () {
            var foodItemName = $(this).val();
            if (foodItemName !== 'all') {
                selectedFoodItems.push(foodItemName);
            }
        });

        // Show or hide rows based on selected food items
        $('.food-row').each(function () {
            var foodItemName = $(this).data('food-item');
            if (selectedFoodItems.length === 0 || selectedFoodItems.includes(foodItemName)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });

        // Show or hide columns based on selected vendors
        $('.vendor-column').each(function () {
            var vendorId = $(this).data('vendor-id');
            if (selectedVendors.length === 0 || selectedVendors.includes(vendorId)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });

        // Hide vendor header columns if the corresponding vendor columns are hidden
        $('th[data-vendor-id]').each(function () {
            var vendorId = $(this).data('vendor-id');
            if (selectedVendors.length === 0 || selectedVendors.includes(vendorId)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    }

    // Use event delegation to handle the change event for vendor and food item filter checkboxes
    $(document).on('change', '.vendor-filter, .food-filter', function () {
        filterComparisonTable();
    });

    // Trigger the initial filtering to ensure the table is correctly displayed on page load
    filterComparisonTable();
    $(document).on('submit', '.approve-quotation-form', function (e) {
        console.log("hi");
        $('#loader').removeClass('d-none');
        e.preventDefault();

        var form = $(this);
        var url = form.attr('action');
        var formData = form.serialize();

        // Disable the submit button to prevent multiple submissions
        form.find('button[type="submit"]').prop('disabled', true);

        $.post(url, formData, function (response) {
            $('#loader').addClass('d-none');

            // Enable the submit button again (optional, depending on your flow)
            form.find('button[type="submit"]').prop('disabled', false);

            // Assuming you're redirecting after success, the loader may remain visible until the page reloads
            if (response.success) {
                window.location.href = '@Url.Action("ViewReceivedQuotations")';
            }
            else {
                alert('Failed to approve the quotation. Please try again later.');
            }
        }).fail(function () {
            $('#loader').addClass('d-none');
            alert('An error occurred while processing your request.');
            form.find('button[type="submit"]').prop('disabled', false); // Re-enable the button on failure
        });
    });



});