window.onload = function () {
    showSeachBar();
    showTable();
};
function showSeachBar() {

    $.ajax({
        url: "Branch/GetSearchValue",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data.value) {
                document.getElementById("branch-search").value = data.value;
            } else {
                console.error("No branch found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving branch:", textStatus, errorThrown);
        }
    });
}

function handleSearch(event) {
    if (event.key === "Enter") {
        const searchValue = document.getElementById("branch-search").value;
        searchBranchList(searchValue);
    }
}

function searchBranchList(value) {

    $.ajax({
        url: "Branch/PostSearchValue",
        dataType: "json",
        type: "POST",
        data: { value: value },
        success: function (data) {
            if (data.success) {
                showTable();
            } else {
                console.error("No branch found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving branch:", textStatus, errorThrown);
        }
    });
}
function handleAdd(event) {
    if (event.key === "Enter") {
        const branchName = document.getElementById("branch-add").value;

        $.ajax({
            url: "Branch/PostBranch",
            dataType: "json",
            type: "POST",
            data: { value: branchName },
            success: function (data) {
                if (data.success) {
                    document.getElementById("branch-add").value = null;
                    showTable();
                } else {
                    console.error("No branch found or unexpected response format.");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error retrieving branch:", textStatus, errorThrown);
            }
        });
    }
}

function showTable() {

    $.ajax({
        url: "Branch/GetBranch",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                const tableBody = document.getElementById("branch-table-body");
                tableBody.innerHTML = "";

                for (let i = 0; i < data.length; ++i) {
                    const tableRow = document.createElement('tr');
                    tableRow.innerHTML = `<td>${data[i].name}</td>
                                          <td><button type="button" class="btn btn-primary" onclick="deleteBranch(${data[i].id})">Delete</button>
                                          </td>`;

                    tableBody.appendChild(tableRow);
                }
            } else {
                console.error("Error retrieving Branch:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}
function deleteBranch(id) {
    $.ajax({
        url: "Branch/DeleteBranch",
        dataType: "json",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data) {
                showTable();
            } else {
                console.error("Error retrieving Branch:", data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error sending AJAX request:", textStatus, errorThrown);
        }
    });
}