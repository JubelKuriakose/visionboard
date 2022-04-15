
//------------------Auto Hide Navbar------------------//
document.addEventListener("DOMContentLoaded", function () {

    autoHide = document.querySelector('.autohide');

    navbarHeight = document.querySelector('.navbar').offsetHeight;
    document.body.style.paddingTop = navbarHeight + 'px';

    if (autoHide) {

        var lastScrollTop = 0;
        window.addEventListener('scroll', function () {
            let scrollTop = window.scrollY;
            if (scrollTop < lastScrollTop) {
                autoHide.classList.remove('scrolled-down');
                autoHide.classList.add('scrolled-up');
            }
            else {
                autoHide.classList.remove('scrolled-up');
                autoHide.classList.add('scrolled-down');
            }
            lastScrollTop = scrollTop;

        });

    }

});
//-------------------*******-----------------------//


//------------------Button back to top------------------//

window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    let mybutton = document.getElementById("btn-back-to-top");

    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

function backToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
//-------------------*******-----------------------//

//------------------GetALLGoalsOfTags------------------//
$("#selectTagsIndex").change(function () {
    GetALLGoalsOfTags($(this).val());
});

function GetALLGoalsOfTags(tagIds) {
    try {
        $.ajax({
            type: 'GET',
            url: "/Goals/Index",
            data: { tagIds: tagIds },
            traditional: true,
            success: function (res) {
                if (res.isValid) {
                    $('#list-goals').html(res.html)
                }
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

//------------------show Create Step poup up------------------// 
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
                        $("#tagDropDownList").append($("<option></option>").val(res.id).text(res.name).attr("selected", "selected"));
                        $("#tagDropDownList").multiselect('rebuild');
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