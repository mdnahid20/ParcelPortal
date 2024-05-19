window.onload = function () {
    showBranch();
};

function showBranch() {
    const senderBranch = document.getElementById("senderBranch");
    const receiverBranch = document.getElementById("receiverBranch");


    $.ajax({
        url: "Courier/GetBranch",
        dataType: "json",
        type: "GET",
        success: function (data) {
            if (data) {
                 
                for (let i = 0; i < data.length; ++i) {
                    const newOption = document.createElement("option");
                    newOption.text = data[i].name;
                    senderBranch.appendChild(newOption);
                }

                for (let i = 0; i < data.length; ++i) {
                    const newOption = document.createElement("option");
                    newOption.text = data[i].name;
                    receiverBranch.appendChild(newOption);
                }

            } else {
                console.error("No Branchs found or unexpected response format.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error retrieving Branchs:", textStatus, errorThrown);
        }
    });
}