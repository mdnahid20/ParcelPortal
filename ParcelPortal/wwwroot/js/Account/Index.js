window.onload = function () {
  //  showSeachBar();
  //  showCheckbox();
    showTable();
};
function showSeachBar() {

    $.ajax({
        url: "Account/GetSearchValue",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                document.getElementById("AccountSearch").value = data.value;
                document.getElementById("AccountSearchBy").value = data.option;
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
    const searchValue = document.getElementById("AccountSearch").value;
    const searchOption = document.getElementById("AccountSearchBy").value;
    searchAccountList(searchOption, searchValue);
}
function handleSearch(event) {
    if (event.key === "Enter") {
        const searchValue = document.getElementById("AccountSearch").value;
        const searchOption = document.getElementById("AccountSearchBy").value;
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
            if (data) {
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

function showCheckbox() {

    $.ajax({
        url: "Account/FavouriteCheckbox",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data.favourite) {
                document.getElementById("favouriteCheckbox").checked = true;
            } else {
                document.getElementById("favouriteCheckbox").checked = false;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving Accounts:", textStatus, errorThrown);
        }
    });
}
function checkbox() {
    const checkbox = document.getElementById("favouriteCheckbox");
    const check = checkbox.checked;

    $.ajax({
        url: "Account/FavouriteCheckbox",
        dataType: "json",
        type: "POST",
        data: { check: check },
        success: function (data) {
            if (data.success) {
                showTable();
            } else {
                window.location.href = `/register`;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving Accounts:", textStatus, errorThrown);
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
                const tableBody = document.getElementById("AccountTableBody");
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

function changeFavourite(id) {
    $.ajax({
        url: "Account/ChangeFavourite",
        dataType: "json",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data.success) {
                showTable();
            } else {
                window.location.href = `/register`;
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