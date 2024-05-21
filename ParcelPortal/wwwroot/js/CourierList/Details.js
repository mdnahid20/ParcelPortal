window.onload = function () {
    showSelectoption();
};

function showSelectoption() {
    const senderBranch = document.getElementById("sender-branch");
    const receiverBranch = document.getElementById("receiver-branch");
    const status = document.getElementById("status");

    fetch('/CourierList/GetBranch')
        .then(response => response.json())
        .then(data => {
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
        })
        .catch(error => {
            console.error('Error:', error);
        });

    fetch('/CourierList/GetBothBranch')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                senderBranch.value = data.senderBranch;
                receiverBranch.value = data.receiverBranch;
                status.value = data.status;
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}