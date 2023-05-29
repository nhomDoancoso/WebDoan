    const bookingForm = document.getElementById('booking-form');
    const phoneNumberInput = document.getElementById('phone-number');
    const submitButton = document.querySelector('.btn-send');
    const phoneNumberRegex = /^(076|090|093|089|012[0-8])+([0-9]{7})$/;

    submitButton.addEventListener('click', (event) => {
        if (phoneNumberInput.value.trim() === '') {
        alert('Bạn chưa nhập số điện thoại');
    event.preventDefault();
    reloadPage();
        } else if (isNaN(phoneNumberInput.value)) {
        alert('Số điện thoại chỉ được nhập số');
    event.preventDefault();
    reloadPage();
        } else if (phoneNumberInput.value.length < 10) {
        alert('Số điện thoại không nhỏ hơn 10 số !');
    event.preventDefault();
    reloadPage();
    location.reload();
        } else if (phoneNumberInput.value.length > 11) {
        alert('Số điện thoại không vượt quá 10 số !');
    event.preventDefault();
    reloadPage();
        }
    else if (!phoneNumberRegex.test(phoneNumberInput.value)) {
        alert('Số điện thoại không đúng định dạng');
    event.preventDefault();
    reloadPage();
        }
    });

    function reloadPage() {
        location.reload();
    }
