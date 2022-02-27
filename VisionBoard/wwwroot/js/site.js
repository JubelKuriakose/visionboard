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
            // to make popup draggable
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
//------------------*****------------------//

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
                    $('#list-tags').html(res.html)
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
                if (res.isValid) {
                    $('#list-rewards').html(res.html)
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

//------------------Delete Tag-----------------//
function DeleteTag(url) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            if (res.success) {
                $('#list-tags').html(res.html);
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

//------------------Delete Rewards-----------------//
function DeleteReward(url) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            if (res.success) {
                $('#list-rewards').html(res.html);
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

