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
            toast: true,
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
        $.getJSON('http://localhost:7071/api/swollenCoffee',{function:'session', Email:$('#txtEmail').val(),Password:$('#txtPassword').val()},function(result){
        objResult = JSON.parse(result); 
        
        if(objResult.Outcome != 'Login Failed'){
                // set your Session Storage Item here
                sessionStorage.setItem('MembershipID', objResult.Outcome);
                // then redirect the user to the dashboard
                window.location.href='index.html';
                //loads QRCode and fills purchase history table
                loadMemberQR(localStorage.getItem('MembershipID'));
                fillPurchaseHistoryTable();
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

function loadMemberQR(MembershipID){
    new QRcode(document.getElementById("divQRcode"),MembershipID);
    $('#spUsername').text(MembershipID);
}
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
    if(!$('#txtCity').val()){
        blnError=true;
        strErrorMessage+= '<p>City is Blank.</p>';
    }
    if(!$('#txtState').val()){
        blnError=true;
        strErrorMessage+= '<p>State is Blank.</p>';
    }
    if(!$('#txtZIP').val()){
        blnError=true;
        strErrorMessage+= '<p>Zip Code is Blank.</p>';
    } else if ($('#txtZIP').val().length < 5 || $('#txtZIP').val().length < 5){
        blnError=true;
        strErrorMessage+= '<p>Zip Code Must be 5 Digits Long.'
    }
    if(!$('#txtPhoneNumber').val()){
        blnError=true;
        strErrorMessage+= '<p>Phone Number is Blank.</p>';
    }
    if(!$('#txtDateOfBirth').val()){
        blnError=true;
        strErrorMessage+= '<p>Birthday is Blank.</p>';
    }
    if(blnError == true){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
        })
    }else{
        var objNewSessionPromise= $.post('http://localhost:7071/api/swollenCoffee?', {function:'membership',strEmail:$('#txtEmail').val(), strFirstName:$('#txtFirstName').val(), strLastName:$('#txtLastName').val(), strPassword:$('#txtPassword').val(), strAddress1:$('txtAddress1').val(),strAddress2:$('txtAddress2').val(),strCity:$('txtCity').val(),strState:$('txtState').val(),strZip:$('txtZIP').val(),strPhoneNumber:$('#txtPhoneNumber').val(),strDateOfBirth:$('#txtDateOfBirth').val()}, function(result){
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

//populates preferred locations
$.getJSON('http://localhost:7071/api/swollencoffee',{function:'location'}, function(result){
        console.log(result);//delete eventually
        $.each(result,(i,location)=>{
            console.log(location.LocationID);
            $('#cboNewPreferredLocation').append('<option value="' + location.locationID + '">' + location.locationID + '</option>');
        }) 
    })

//start for index.html
$(document).on('click','#btnSignOut', function(){
    let blnError = false;
    let strErrorMessage = '';
    
    window.location.href='login.html';
        $.post('http://localhost:7071/api/swollenCoffee',{function:'session',strEmail:$('#txtEmail').val(),strPassword:$('#txtPassword').val()},function(result){
        objResult = JSON.parse(result); //should update last used date and time
        
        if(objResult.Outcome != 'Login Failed'){
                // set your Session Storage Item here
                sessionStorage.setItem('MembershipID', objResult.Outcome);
                // then redirect the user to the dashboard
                window.location.href='index.html';
                
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    html: '<p>The provided username and password did not match any in our database</p>'
                })
            }
        })
    
})

$(document).on('click','#btnToggleExisting', function(){
    $('#divPurchaseHistroy').slideToggle();
})

$(document).on('click','#btnUpdateInformation', function(){
    $.getJSON('http://localhost:7071/api/swollencoffee?function=membership&SessionID='+localStorage.getItem(MembershipID),function(result){
        $.each(result,(i,member)=>{
            $('#txtUpdateEmail').val(member.email);
            $('#txtUpdateFirstName').val(member.firstName);
            $('#txtUpdateLastName').val(member.lastName);
            $('#txtUpdateDateOfBirth').val(member.dateOfBirth);
            $('#txtUpdatePhone').val(member.areaCOde + member.telephoneNumber);
            $('#txtUpdatePhone').attr('data-phoneid',member.phoneID);
            $('#txtUpdateAddress1').val(member.street1);
            $('#txtUpdateAddress1').attr('data-addressid',member.addressID);
            $('#txtUpdateAddress2').val(member.street2);
            $('#txtUpdateCity').val(member.city);
            $('#txtUpdateState').val(member.state);
            $('#txtUpdateZip').val(member.zip);
            $('#cboNewPreferredLocation').val(member.preferredLocation);
        })
    })
    
    $('#divUpdateInfo').slideToggle();
})

$(document).on('click', '#btnSubmitUpdate', function(){
    let strErrorMessage='';
    let blnError = false;
    if(!$('#txtEmail').val()){
        blnError=true;
        strErrorMessage+= '<p>Email is Blank.</p>';
    }else if(!isValidEmail($('#txtEmail').val())){
        blnError=true;
        strErrorMessage+='<p>Email is not valid</p>';
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
    if(!$('#txtCity').val()){
        blnError=true;
        strErrorMessage+= '<p>City is Blank.</p>';
    }
    if(!$('#txtState').val()){
        blnError=true;
        strErrorMessage+= '<p>State is Blank.</p>';
    }
    if(!$('#txtZIP').val()){
        blnError=true;
        strErrorMessage+= '<p>Zip Code is Blank.</p>';
    } else if ($('#txtZIP').val().length < 5 || $('#txtZIP').val().length < 5){
        blnError=true;
        strErrorMessage+= '<p>Zip Code Must be 5 Digits Long.'
    }
    if(!$('#txtPhoneNumber').val()){
        blnError=true;
        strErrorMessage+= '<p>Phone Number is Blank.</p>';
    }
    if(!$('#txtDateOfBirth').val()){
        blnError=true;
        strErrorMessage+= '<p>Birthday is Blank.</p>';
    }
    if(blnError == true){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
        })
    }else{ // this needs to be a put not a post.
        var objNewSessionPromise= $.post('http://localhost:7071/api/swollenCoffee?', {function:'membership',Email:$('#txtEmail').val(), strFirstName:$('#txtFirstName').val(), strLastName:$('#txtLastName').val(), strPassword:$('#txtPassword').val(), strAddress1:$('txtAddress1').val(),strAddress2:$('txtAddress2').val(),strCity:$('txtCity').val(),strState:$('txtState').val(),strZip:$('txtZIP').val(),strPhoneNumber:$('#txtPhoneNumber').val(),strDateOfBirth:$('#txtDateOfBirth').val()}, function(result){
            console.log(JSON.parse(result).Outcome);
            objNewSessionPromise = JSON.parse(result);
        }) 
        $.when(objNewSessionPromise).done(function(){
            if(objNewSessionPromise.Outcome == 'Error'){
                Swal.fire({
                    icon:'error',
                    title:'Login Failed',
                    html: '<p> Review Username and Password </p>'
                })
            } else{ 
                console.log(objNewSessionPromise);//for error checking
                Swal.fire({
                    icon: 'success',
                    title: 'Update Successful!',
                    showClass: {
                        popup: `
                        animate__animated
                        animate__fadeInDown
                        animate__faster
                        `
                    }
                })
            }
        })
    }
})

$(document).on('click','#btnViewHistory',function(){
    $('#divPurchaseHistory').slideToggle();
    fillPurchaseHistoryTable();
})

/*
    allows user to click a transaction id and produces a sweetalert

$(document).on('click','#btnTransaction', function(){
    let strTransID=$(this).attr('TransactionID');

    Swal.fire({
        icon: 'info',
        title: '<p>'+ strTransID.TransactionID+ '</p>',
        html: '<p>'+ strTransID.TransactionDateTime+'</p>',
        forEach(strTransID.ItemID => {
            html: '<p>' + strTransID.+ '</p>'
        });
        
    })
})*/
//end of updating customer information

function fillPurchaseHistoryTable(){
    $('#tblPurchaseHistory tbody').empty();
    let strCurrentSessionID = sessionStorage.getItem('MembershipID');
            /*let strTableHTML = '<tr><td> <button type="button" class="btn col-12" id="btnTransaction">  x42ghue78 </button></td></tr>';
            console.log("here");
            $('#tblPurchaseHistory tbody').append(strTableHTML);*/
    $.getJSON('http://localhost:7071/api/swollenCoffee',{function: 'purchases', SessionID:strCurrentSessionID},function(result){
        $.each(result,function(i,transaction){
            let strTableHTML = '<tr><td> <button type="button" class="btn" id="btnTransaction"> '+  transaction.TransactionID+'</button></td></tr>';
            $('#tblPurchaseHistory tbody').append(strTableHTML);
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
    if(sessionStorage.getItem('MembershipID')){
        let strCurrentSessionID = sessionStorage.getItem('MembershipID')
        $.getJSON('http://localhost:7071/api/swollenCoffee', {function:'session', strSessionID: strCurrentSessionID}, function(result){
            if(result.Outcome != 'Valid Session'){
                return false;
            } else {
                return true;
            }
        })
    }else {
        return false;
    }
}
// End Universal Functions

