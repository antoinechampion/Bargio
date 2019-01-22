$(document).ready(function() {
    upload_begin = false;
    $("#sendXml").click(function(e) {
        if (upload_begin) return;
        if ($("#fileInput").get(0).files.length === 0) return;
        upload_begin = true;
        $("#info-attente").addClass("d-none");
        $("#info-patience").removeClass("d-none");
        $("#info-migration").removeClass("d-none");
        $("#sendXml").addClass("disabled");
        $("#fileInput").addClass("disabled");

        const fdata = new FormData();
        fdata.append("xml_file", $("#fileInput")[0].files[0]);
        fdata.append("append_to_db", $("#append").is(":checked"));
        $.ajax({
            type: "POST",
            url: "/Api/ImportDb",
            data: fdata,
            contentType: false,
            processData: false,
            success: function(response) {
                if (response.charAt(0) === "0") {
                    response = response.substr(1);
                    $("#info-migration").addClass("d-none");
                    $("#info-patience").addClass("d-none");
                    $("#message-validation").append(response);
                    $("#message-validation").addClass("bg-warning text-dark");
                    $("#info-succes").removeClass("d-none");
                } else {
                    $("#info-migration").addClass("d-none");
                    $("#info-patience").addClass("d-none");
                    $("#message-validation").append(response);
                    $("#message-validation").addClass("bg-danger text-white");
                    $("#info-echec").removeClass("d-none");
                }
            }
        });
    });
});