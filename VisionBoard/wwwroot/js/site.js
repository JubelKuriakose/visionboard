// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// show Create Step poup up
showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');

            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}
//------------------*****------------------//

//------------------Add or Edit Step------------------//

AddorEditStep = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $(".steps-table").html(res.html)
                    $('#list-steps').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}


//------------------ Add or Edit Measuerment------------------//

AddorEditMeasurement = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#list-measurement').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//------------------*****------------------//

//------------------Add or Edit Tag------------------//
AddorEditTag = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    if (res.source == "DropDown") {
                        if ($("#TagId option:selected").val() == res.id) {
                            $("#TagId option:selected").remove();
                        }
                        $("#TagId").append($("<option></option>").val(res.id).text(res.name).attr("selected", "selected"));
                    }
                    else {
                        $('#list-tags').html(res.html)
                    }

                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//------------------*****------------------//

//------------------Edit Tag From dropdown------------------//
EditTagFromDropdown = (url, title) => {
    var id = $("#TagId").children(":selected").attr("value");
    url = url + "&id=" + id;
    showInPopup(url, title);
}
//------------------*****------------------//

//------------------Add or Edit Reward------------------//
AddorEditReward = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log(res);
                if (res.isValid) {
                    if (res.source == "DropDown") {
                        if ($("#RewardId option:selected").val() == res.id) {
                            $("#RewardId option:selected").remove();
                        }
                        $("#RewardId").append($("<option></option>").val(res.id).text(res.name).attr("selected", "selected"));
                    }
                    else {
                        $('#list-rewards').html(res.html)
                    }

                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');

                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//------------------*****------------------//

//------------------Edit Reward From dropdown------------------//
EditRewardFromDropdown = (url, title) => {
    var id = $("#RewardId").children(":selected").attr("value");
    url = url + "&id=" + id;
    showInPopup(url, title);
}
//------------------*****------------------//

//------------------Delete Tag-----------------//
function DeleteTag(url) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            if (res.isValid) {
                if (res.source == "DropDown") {
                    $("#TagId option:selected").remove();
                }
                else {
                    $('#list-tags').html(res.html);
                }
            }
            else {
                $.notify(res.message, "error");
            }
        }
    });
    $('#form-modal .modal-body').html('');
    $('#form-modal .modal-title').html('');
    $('#form-modal').modal('hide');
}
//------------------*****------------------//

//------------------Delete Reward From dropdown------------------//
DeleteTagFromDropdown = (url, title) => {
    var id = $("#TagId").children(":selected").attr("value");
    url = url + "&id=" + id;
    showInPopup(url, title);
}
//------------------*****------------------//

//------------------Delete Rewards-----------------//
function DeleteReward(url) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            if (res.isValid) {
                if (res.source == "DropDown") {
                    $("#RewardId option:selected").remove();
                }
                else {
                    $('#list-rewards').html(res.html);
                }
            }
            else {
                $.notify(res.message, "error");
            }
        }
    });
    $('#form-modal .modal-body').html('');
    $('#form-modal .modal-title').html('');
    $('#form-modal').modal('hide');
}
//------------------*****------------------//

//------------------Delete Reward From dropdown------------------//
DeleteRewardFromDropdown = (url, title) => {
    var id = $("#RewardId").children(":selected").attr("value");
    url = url + "&id=" + id;
    showInPopup(url, title);
}
//------------------*****------------------//

//------------------Delete Measurement-----------------//
function DeleteMeasurement(url) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            if (res.success) {
                $('#list-measurement').html(res.html);
            }
            else {
                $.notify(res.message, "error");
            }
        }
    });
    $('#form-modal .modal-body').html('');
    $('#form-modal .modal-title').html('');
    $('#form-modal').modal('hide');
}
//------------------*****------------------//

//------------------Delete Step-----------------//
function DeleteStep(url) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            if (res.success) {
                $('#list-steps').html(res.html);
                $(".steps-table").html(res.html)
            }
            else {
                $.notify(res.message, "error");
            }
        }
    });
    $('#form-modal .modal-body').html('');
    $('#form-modal .modal-title').html('');
    $('#form-modal').modal('hide');
}
//------------------*****------------------//