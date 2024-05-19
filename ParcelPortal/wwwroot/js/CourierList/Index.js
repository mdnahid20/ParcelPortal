window.onload = function () {
    showSeachBar();
    showTable();
};
function showSeachBar() {

    $.ajax({
        url: "CourierList/GetSearchValue",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                document.getElementById("courier-search").value = data.value;
                document.getElementById("search-by").value = data.option;
            } else {
                console.error("No CourierLists found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving CourierLists:", textStatus, errorThrown);
        }
    });
}

function handleChoiceChange() {
    const searchValue = document.getElementById("courier-search").value;
    const searchOption = document.getElementById("search-by").value;
    searchCourierListList(searchOption, searchValue);
}
function handleSearch(event) {
    if (event.key === "Enter") {
        const searchValue = document.getElementById("courier-search").value;
        const searchOption = document.getElementById("search-by").value;
        searchCourierListList(searchOption, searchValue);
    }
}

function searchCourierListList(option, value) {

    $.ajax({
        url: "CourierList/PostSearchValue",
        dataType: "json",
        type: "POST",
        data: { option: option, value: value },
        success: function (data) {
            if (data) {
                showTable();
            } else {
                console.error("No CourierLists found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving CourierLists:", textStatus, errorThrown);
        }
    });
}

function showTable() {

    $.ajax({
        url: "CourierList/GetCourierList",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                const tableBody = document.getElementById("courierList-table-body");
                tableBody.innerHTML = "";

                for (let i = 0; i < data.length; ++i) {
                    const tableRow = document.createElement('tr');
                    tableRow.innerHTML = `<td>${data[i].consignmentNumber}</td>
                                          <td>${data[i].status}</td>
                                          <td><button type="button" class="btn btn-primary" onclick="deleteCourierList(${data[i].id})">Delete</button>
                                          <button  class="btn btn-primary" onclick="details(${data[i].id})">Details</button> 
                                          </td>`;

                    tableBody.appendChild(tableRow);
                }
            } else {
                console.error("Error retrieving CourierList:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function deleteCourierLis(id) {

    $.ajax({
        url: "CourierList/PostCourier",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if(data.success)
            showTable();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}
function details(id) {
    window.location.href = `/CourierList/Details/${id}`;
}