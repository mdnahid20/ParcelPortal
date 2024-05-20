window.onload = function () {
    showSeachBar();
    showPagination();
    showTable();

};
function showSeachBar() {

    $.ajax({
        url: "Account/GetSearchValue",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                document.getElementById("account-search").value = data.searchValue;
                document.getElementById("account-search-by").value = data.userAttribute;
            } else {
                console.error("No Accounts found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving Accounts:", textStatus, errorThrown);
        }
    });
}

function handleChoiceChange() {
    const searchValue = document.getElementById("account-search").value;
    const searchOption = document.getElementById("account-search-by").value;
    searchAccountList(searchOption, searchValue);
}
function handleSearch(event) {
    if (event.key === "Enter") {
        const searchValue = document.getElementById("account-search").value;
        const searchOption = document.getElementById("account-search-by").value;
        searchAccountList(searchOption, searchValue);
    }
}

function searchAccountList(option, value) {

    $.ajax({
        url: "Account/PostSearchValue",
        dataType: "json",
        type: "POST",
        data: { option: option, value: value },
        success: function (data) {
            if (data.success) {
                showPagination();
                showTable();
            } else {
                console.error("No Accounts found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving Accounts:", textStatus, errorThrown);
        }
    });
}

function showPagination() {
    $.ajax({
        url: "Account/GetPageNumber",
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
                console.error("Error retrieving Account:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function handlePage(page) {

    $.ajax({
        url: "Account/PostPageNumber",
        dataType: "json",
        type: "Post",
        data: { page : page },
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
        url: "Account/GetAccount",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                const tableBody = document.getElementById("account-table-body");
                tableBody.innerHTML = "";

                for (let i = 0; i < data.length; ++i) {
                    const tableRow = document.createElement('tr');
                    tableRow.innerHTML = `<td>${data[i].name}</td>
                                          <td>${data[i].email}</td>
                                          <td>${data[i].role}</td>
                                          <td><button type="button" class="btn btn-primary" onclick="deleteAccount(${data[i].id})">Delete</button>
                                          <button  class="btn btn-primary" onclick="details(${data[i].id})">Details</button> 
                                          </td>`;

                    tableBody.appendChild(tableRow);
                }
            } else {
                console.error("Error retrieving Account:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}

function details(id) {
    window.location.href = `/Account/Details/${id}`;
}