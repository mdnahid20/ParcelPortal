function handlEvent(event) {
    if (event.key === "Enter") {

        const consignmentNumber = document.getElementById("inputField").value;

        $.ajax({
            url: "TrackProduct/ConsigmentNumber",
            dataType: "json",
            type: "Post",
            data: { value: consignmentNumber },
            success: function (data) {
                if (data.success) {

                    document.getElementById("status").textContent = data.courier.status;
                    document.getElementById("deliveryTime").textContent = data.courier.deliveryTime;
                    document.getElementById("senderBranch").textContent = data.courier.senderBranch;
                    document.getElementById("receiverBranch").textContent = data.courier.receiverBranch;
                    document.getElementById("quantity").textContent = data.courier.productQuantity
                } else {
                    document.getElementById("consigmentMessage").textContent = "Wrong consigment number";
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error retrieving branch:", textStatus, errorThrown);
            }
        });
    }
}