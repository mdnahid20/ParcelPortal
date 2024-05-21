window.onload = function () {
    showSeachBar();
    showPagination();
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
function showPagination() {
    $.ajax({
        url: "CourierList/GetPageNumber",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data.success) {
                const previousOne = document.getElementById("page-previous-one");
                const previousTwo = document.getElementById("page-previous-two");
                const nextOne = document.getElementById("page-next-one");
                const nextTwo = document.getElementById("page-next-two");

                if (data.previousOne == -1) {
                    previousOne.style.display = 'none';
                    previousTwo.style.display = 'none';
                } else if (data.previousTwo == "-1") {
                    previousOne.style.display = '';
                    previousTwo.style.display = 'none';
                } else {
                    previousOne.style.display = '';
                    previousTwo.style.display = '';
                }


                if (data.nextOne == -1) {
                    nextOne.style.display = 'none';
                    nextTwo.style.display = 'none';
                } else if (data.nextTwo == -1) {
                    nextOne.style.display = '';
                    nextTwo.style.display = 'none';
                } else {
                    nextOne.style.display = '';
                    nextTwo.style.display = '';
                }

                previousOne.textContent = data.previousOne;
                previousTwo.textContent = data.previousTwo;
                nextOne.textContent = data.nextOne;
                nextTwo.textContent = data.nextTwo;
            } else {
                console.error("Error retrieving Branch:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function handlePage(page) {

    $.ajax({
        url: "CourierList/PostPageNumber",
        dataType: "json",
        type: "Post",
        data: { page: page },
        success: function (data) {
            if (data.success) {
                showTable();
                showPagination();
            }
            else {
                console.error("Error retrieving Branch:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
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

function deleteCourierList(id) {

    $.ajax({
        url: "CourierList/PostCourier",
        dataType: "json",
        type: "Post",
        data: { id: id },
        success: function (data) {
            if (data.success) {
                showTable();
                showPagination();
            }
            else {
                console.error("Error retrieving Branch:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function details(id) {
    DetailsId = id;
    window.location.href = `/CourierList/Details/${id}`;
}