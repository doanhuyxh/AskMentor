var role = "";

$(document).ready(function () {
    $(".items-signup").click(function (event) {
        $(".items-signup").removeClass("active");
        $(this).addClass("active");
        $(".circle-main, .inner-circle").removeClass("active");
        $(this).find(".circle-main, .inner-circle").addClass("active");
        $("[data-qa='btn-apply']").removeAttr("disabled");
        $("[data-qa='btn-apply']").addClass("active");
        var dataCV = $(this).attr("data-cv");
        var dataButton = $(this).attr("data-button");
        $("[data-qa='btn-apply']").text(dataButton);
        console.log(dataCV);
        if (dataCV === 'client') {
            role = "Leaner"
        } else {
            role = "Mentor"
        }
        event.stopPropagation();
    });
    $("[data-qa='btn-apply']").click(function (event) {
        $("#createAccount").hide();
        $(".page-res").show();
        $(".right-header").show().css("display", "flex");

    })
});

function CreateAcc() {

    // Lấy thông tin từ form
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;

    // Gửi yêu cầu đăng ký đến API
    fetch('/api/Authencation/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            Email: email,
            Password: password,
            Role: role,
            Name: `${firstName} ${lastName}`
        })
    })
        .then(response => response.json())
        .then(data => {
            // Xử lý dữ liệu trả về từ API ở đây
            console.log(data);
            alert(data.message)
            setTimeout(() => {
                window.location.href = "/Auth/Login"
            }, 1000)
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
