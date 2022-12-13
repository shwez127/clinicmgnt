// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Sticky Navigation Menu JS Code

$(document).ready(function () {
    $('#myTable').DataTable({
        lengthMenu: [
            [3, 6],
            [3, 6],

        ],
    });
});

$(document).ready(function () {
    $('#example').DataTable({
        order: [[0, 'desc']],
        lengthMenu: [
            [3, 6],
            [3, 6],

        ],
    });
});

function deleteFunction() {
    let val = confirm("Are you sure you want to Delete?");
    if (val == false) {
        return event.preventDefault();
    }
}

function addFunction() {
    let val = confirm("Are you sure you want to Save?");
    if (val == false) {
        return event.preventDefault();
    }
}


function editFunction() {
    let val = confirm("Are you sure you want to Edit?");
    if (val == false) {
        return event.preventDefault();
    }
}

function confirmFunction() {
    let val = confirm("Are you sure you want to book an appointment?");
    if (val == false) {
        return event.preventDefault();
    }
}

function cancelFunction() {
    let val = confirm("Are you sure you want to cancel?");
    if (val == false) {
        return event.preventDefault();
    }
}



var doc = new jsPDF();
var specialElementHandlers = {
    '#editor': function (element, renderer) {
        return true;
    }
};

$('#cmd').click(function () {
    doc.fromHTML($('#content').html(), 15, 15, {
        'width': 170,
        'elementHandlers': specialElementHandlers
    });
    doc.save('sample-file.pdf');
});


function submitFeedback() {
    let val = confirm("Are you sure you want to submit?");
    if (val == false) {
        return event.preventDefault();
    }
}

function confirmPayment() {
    let val = confirm("Are you sure you want to pay?");
    if (val == false) {
        return event.preventDefault();
    }
}