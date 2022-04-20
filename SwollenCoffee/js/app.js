//start for login.html
$(document).on('click','#btnLogin', function(){
    let blnError = false;
    let strErrorMessage = '';
    if(!$('#txtEmail').val()){
        blnError = true;
        strErrorMessage += '<p>Please provide an email address to continue</p>';
    }else if(!isValidEmail($('#txtEmail').val())){
        blnError=true;
        strErrorMessage+='<p>Check Email Format</p>';
    }
    if(!$('#txtPassword').val()){
        blnError = true;
        strErrorMessage += '<p>Please provide your password to continue</p>';
    }else if(!isValidPassword($('#txtPassword').val())){
        
        blnError=true;
        strErrorMessage+='<p>Check Password Format</p>';
    }
    if(blnError == true){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage,
            showClass: {
                popup: `
                animate__animated
                animate__fadeInDown
                animate__faster
                `
            }
        })
    } else{
        $.post('https://www.swollenhippo.com/DS3870/Comics/createSession.php',{strEmail:$('#txtEmail').val(),strPassword:$('#txtPassword').val()},function(result){
        objResult = JSON.parse(result); 
        
        if(objResult.Outcome != 'Login Failed'){
                // set your Session Storage Item here
                sessionStorage.setItem('CharacterSession', objResult.Outcome);
                // then redirect the user to the dashboard
                window.location.href='index.html';
                fillCharacterTable();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    html: '<p>Your email address and password are not an account</p>'
                })
            }
        })
    }
})
//end of login.html

//start for newAccount.html
$(document).on('click','#btnNewAccount', function(){
    let strErrorMessage='';
    let blnError = false;
    if(!$('#txtEmail').val()){
        blnError=true;
        strErrorMessage+= '<p>Email is Blank.</p>';
    }else if(!isValidEmail($('#txtEmail').val())){
        blnError=true;
        strErrorMessage+='<p>Email is not valid</p>';
    }
    if(!doPasswordsMatch($('#txtPassword').val(),$('#txtVerifyPassword').val())){
        blnError=true;
        strErrorMessage+='<p>Passwords do not match</p>';
    }
    if(!$('#txtPassword').val()){
        blnError=true;
        strErrorMessage+= '<p>Password is Blank.</p>';
    }else if(!isValidPassword($('#txtPassword').val())){
        blnError=true;
        strErrorMessage+='<p>Password is not valid</p>';
    }
    if(!$('#txtFirstName').val()){
        blnError=true;
        strErrorMessage+= '<p>First Name is Blank.</p>';
    }
    if(!$('#txtLastName').val()){
        blnError=true;
        strErrorMessage+= '<p>Last Name is Blank.</p>';
    }
    if(!$('#txtAddress1').val()){
        blnError=true;
        strErrorMessage+= '<p>Address 1 is Blank.</p>';
    }
    if(!$('#txtAddress2').val()){
        blnError=true;
        strErrorMessage+= '<p>Address 2 is Blank.</p>';
    }
    if(!$('#txtPhoneNumber').val()){
        blnError=true;
        strErrorMessage+= '<p>Phone Number is Blank.</p>';
    }
    if(!$('#txtDateOfBirth').val()){
        blnError=true;
        strErrorMessage+= '<p>Date of Birth is Blank.</p>';
    }
    if(blnError == true){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
        })
    }else{
        var objNewSessionPromise= $.post('insert create account endpoint', { strEmail:$('#txtEmail').val(), strFirstName:$('#txtFirstName').val(), strLastName:$('#txtLastName').val(), strPassword:$('#txtPassword').val(), strAddress:$('txtAddress').val(),strPhoneNumber:$('#txtPhoneNumber').val(),strDateOfBirth:$('#txtDateOfBirth').val()}, function(result){
            console.log(JSON.parse(result).Outcome);
            objNewSessionPromise = JSON.parse(result);
        }) 

        $.when(objNewSessionPromise).done(function(){
            if(objNewSessionPromise.Outcome == 'Login Failed'){
                Swal.fire({
                    icon:'error',
                    title:'Login Failed',
                    html: '<p> Review Username and Password </p>'
                })
            } else{ 
                sessionStorage.setItem('MembershipID', objNewSessionPromise.Outcome);
                console.log(objNewSessionPromise);
                Swal.fire({
                    icon: 'success',
                    title: 'Acccount Created!',
                    html: '<p>Please Login to access your account and MembershipID</p>',
                    showClass: {
                        popup: `
                        animate__animated
                        animate__fadeInDown
                        animate__faster
                        `
                    }
                })
                window.location.href='login.html'; //window.location.replace removes from history
            }
        })
    }
})

$(document).on('click','#btnBackToLogin', function(){
    window.location.href = 'login.html';
})
//end for newAccount.html

$(document).on('click','#btnAddCharacter', function(){
    let strSesID= sessionStorage.getItem('CharacterSession');

    $.getJSON('https://www.swollenhippo.com/DS3870/Comics/verifySession.php', {strSessionID: strSesID}, function(result){
        console.log(result);
        if(result.Outcome == 'Valid Session'){
            let strCName = $('#txtCharacterName').val();
            let strSPower = $('#txtSuperPower').val();
            let strHLocation = $('#txtLocation').val();
            let strHStaturs = $('#selectStatus').val()
            $.post('https://www.swollenhippo.com/DS3870/Comics/addCharacter.php', {strSessionID: strSesID, strName:strCName, strSuperPower:strSPower, strLocation:strHLocation, strStatus:strHStaturs}, function(result){
                let object = JSON.parse(result);
                if(object.Outcome != 'Error'){
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title:'Task Added.',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    clearCharacterFields()
                    fillCharacterTable();
                } else{
                    Swal.fire({
                        icon: 'error',
                        title: 'Title Not Added',
                        html: '<p> Verify your task data and try again.</p>'
                    })
                }
            })
        } else{
            Swal.fire({
                icon: 'error',
                title: 'Expired Session',
                html: '<p>Oops, appears your session has expired.</p>'
            }).then((result)=>{
                sessionStorage.removeItem('HippoTaskID');
                window.location.href = 'login.html';
            })
        }
    })
})

//start for index.html
$(document).on('click','#btnSignOut', function(){
    let blnError = false;
    let strErrorMessage = '';
    
    window.location.href='login.html';
        /*$.post('https://www.swollenhippo.com/DS3870/Comics/createSession.php',{strEmail:$('#txtEmail').val(),strPassword:$('#txtPassword').val()},function(result){
        objResult = JSON.parse(result); 
        
        if(objResult.Outcome != 'Login Failed'){
                // set your Session Storage Item here
                sessionStorage.setItem('CharacterSession', objResult.Outcome);
                // then redirect the user to the dashboard
                window.location.href='index.html';
                fillCharacterTable();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    html: '<p>The provided username and password did not match any in our database</p>'
                })
            }
        }) */
    
})

$(document).on('click','#btnToggleExisting', function(){
    $('#divPurchaseHistroy').slideToggle();
})

$(document).on('click','#btnUpdateInformation', function(){
    $('#divUpdateInfo').slideToggle();

    $.getJSON('http://localhost:7071/')
})

//for updating customer information
$(document).on('click', '#btnSubmitEmail', function(){
    let strErrorMessage='';
    let blnError = false;

    if(!$('#txtNewEmail').val()){
        blnError = true;
        strErrorMessage += '<p> New Email Address is empty.</p>';
    }
    
    if(blnError == true){
        Swal.fire({
            icon: 'warning',
            title: 'Unable to Update',
            html: strErrorMessage
        })
    }
})

$(document).on('click','#dropdownPrefLocation', function(){
    let strCurrentSessionID = sessionStorage.getItem('membershipID');
    $.getJSON('http://localhost:7071/api/location?',{}, function(result){
        foreach()
    })
})

$(document).on('click','#btnViewHistory',function(){

})
//end of updating customer information

function fillPurchaseHistoryTable(){
    $('#tblPurchaseHistory tbody').empty();
    let strCurrentSessionID = sessionStorage.getItem('membershipID');
    $.getJSON('http://localhost:7071/api/location?',{strSessionID:strCurrentSessionID},function(result){
        $.each(result,function(i,transaction){
            let strTableHTML = '<tr><td>' + transaction.TransactionID + '</td></tr>';
            $('#tblCharacters tbody').append(strTableHTML);
        })
    })
}
//end for index.html


// End Page Specific Functions


// Begin Helper Functions
function isValidEmail(strEmailAddress){
    let regEmail = /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/;
    return regEmail.test(strEmailAddress);
}

function isValidPassword(strPassword){
    let regPassword = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,64}$/;
    return regPassword.test(strPassword);
}

function doPasswordsMatch(strPassword, strVerifyPassword){
    if(strPassword == strVerifyPassword){
        return true;
    } else {
        return false;
    }
}
// End Helper Functions

// Begin Universal Functions
function verifySession(){
    if(sessionStorage.getItem('CharacterSession')){
        let strCurrentSessionID = sessionStorage.getItem('CharacterSession')
        $.getJSON('https://www.swollenhippo.com/DS3870/Test1/verifySession.php', {strSessionID: strCurrentSessionID}, function(result){
            if(result.Outcome != 'Valid Session'){
                return false;
            } else {
                return true;
            }
        })
    } else {
        return false;
    }
}
// End Universal Functions

