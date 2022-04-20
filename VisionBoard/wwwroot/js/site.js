
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
//------------------*****------------------//

//------------------AddOrRemoveMesurement-----------------//

$('input:radio[name="TrackProgress"]').change(
    function () {
        if ($(this).is(':checked') && $(this).val() == 'Yes') {
            var url = '/Measurements/Create?goalId=' + $('#GoalId').val();
            showInPopup(url, 'Add Mesurement');
        }
        else {
            var url = '/Measurements/Delete?id=' + $('#MeasurementId').val();
            showInPopup(url, 'Delete Mesurement');
        }
    });

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
                    $('#show-progress').html(res.html)
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
                $('#show-progress').hide();
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

//------------------Crop Goal Image------------------//
$('#form-modal').on('shown.bs.modal', function () {
    $('#main-cropper').croppie('destroy');
    var basic = $('#main-cropper').croppie
        ({
            viewport: { width: 310, height: 259 },
            boundary: { width: 390, height: 327 },
            url: '/images/noimage.png',
            format: 'png' //'jpeg'|'png'|'webp'
        });

    // Change Event to Read file content from File input
    $('#select-image').on('change', function () {
        readFile(this);
    });

    // Upload button to Post Cropped Image to Store.
    $('#btnupload').on('click', function () {
        $('#main-cropper').croppie('result', 'blob').then(function (blob) {
            var formData = new FormData();

            formData.append('filename', 'FileName.png');
            formData.append('blob', blob);
            var myAppUrlSettings =
            {
                MyUsefulUrl: 'CustomCrop'
            }

            var request = new XMLHttpRequest();
            request.open('POST', myAppUrlSettings.MyUsefulUrl);
            request.send(formData);
            request.onreadystatechange = function () { // Call function when the state changes.
                if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
                    var response = JSON.parse(request.responseText);

                    if (response.message == "SUCCESS") {
                        $("#goal-image").attr("src", "/images/" + response.selectedImage);
                        $("#PictureUrl").val(response.selectedImage);
                        $('#form-modal .modal-body').html('');
                        $('#form-modal .modal-title').html('');
                        $('#form-modal').modal('hide');
                    }
                    else if (response.message == "ERROR") {
                        alert('Failed to crop the image');
                    }
                }
            }
        });
    });
});


//Reading the contents of the specified Blob or File
function readFile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#main-cropper').croppie('bind', {
                url: e.target.result
            });
        }
        reader.readAsDataURL(input.files[0]);
    }
}
//------------------*****------------------//
