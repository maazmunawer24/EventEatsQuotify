// Helper method to generate HTML for the food items grid
function GenerateFoodItemsGridHtml(foodItems) {
    var sb = [];

    if (foodItems != null && foodItems.length > 0) {
        sb.push('<div class="mt-5">');
        sb.push('<h2>Uploaded Food Items</h2>');

        sb.push('<table class="table">');
        sb.push('<thead><tr><th>Food Item</th><th>Price Per KG (Rupees)</th><th>Action</th></tr></thead>');
        sb.push('<tbody>');

        for (var i = 0; i < foodItems.length; i++) {
            var foodItem = foodItems[i];
            sb.push('<tr>');
            sb.push('<td>' + foodItem.Name + '</td>');
            sb.push('<td>' + foodItem.Price + '</td>');
            sb.push('<td>');
            sb.push('<div class="dropdown">');
            sb.push('<button class="btn btn-secondary dropdown-toggle" type="button" id="contextMenu" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Actions</button>');
            sb.push('<div class="dropdown-menu" aria-labelledby="contextMenu">');
            sb.push('<a class="dropdown-item" href="#" onclick="editFoodItem(' + foodItem.Id + ')">Edit</a>');
            sb.push('<a class="dropdown-item" href="#" onclick="deleteFoodItem(' + foodItem.Id + ')">Delete</a>');
            sb.push('</div></div></td></tr>');
        }

        sb.push('</tbody></table></div>');
    } else {
        sb.push('<div class="mt-5"><p>No food items added.</p></div>');
    }

    return sb.join('');
}

// Function to edit food item
function editFoodItem(foodItemId) {
    // Implement the logic to edit the food item (e.g., open a modal)
    console.log('Edit Food Item:', foodItemId);
}

// Function to delete food item
function deleteFoodItem(foodItemId) {
    // Implement the logic to delete the food item
    console.log('Delete Food Item:', foodItemId);
}

// AJAX function to dynamically upload food item and update the grid
function uploadFoodItem() {
    debugger;  // Add this line for debugging
    var form = document.getElementById("uploadForm");
    var formData = new FormData(form);
    
    fetch('/FoodItem/Upload', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            // Check if the response is in JSON format
            return response.json().catch(() => null);
        })
        .then(data => {
            if (data) {
                // Update the grid with the new food item
                var foodItemsContainer = document.getElementById("foodItemsContainer");
                foodItemsContainer.innerHTML = GenerateFoodItemsGridHtml(data.foodItems);

                // Display upload success message
                var successMessage = document.getElementById("uploadSuccessMessage");
                if (successMessage) {
                    successMessage.style.display = "block";
                }
            } else {
                // Handle non-JSON response (if needed)
                console.error('Error: Response is not in JSON format');
            }
        })
        .catch(error => console.error('Error:', error));
}
