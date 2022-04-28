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
        $.post('http://localhost:7071/api/swollenCoffee?function=session&Email='+$('#txtEmail').val()+'&Password='+$('#txtPassword').val(),function(result){
            let objResult = JSON.parse(result);
            console.log(objResult);
            if(objResult != 'Login Failed'){
                let SessionID= objResult.SessionID.split('|')[0];
                let MembershiID= objResult.SessionID.split('|')[1];
                localStorage.setItem('MembershipID', MembershiID);
                localStorage.setItem('SessionID', SessionID);
                $.ajax({
                    method:'PUT',
                    url:'http://localhost:7071/api/swollenCoffee?function=session&SessionID=' + localStorage.getItem('SessionID')
                }).done(function(result){
                    console.log(result);
                    let objResult = JSON.parse(result);
                    if(objResult.Outcome == 'Session Updated'){
                        window.location.href='index.html';
                        loadMemberQR(localStorage.getItem('MembershipID'));
                    }
                    
                })

                //window.location.href='index.html';
                loadMemberQR(localStorage.getItem('MembershipID'));
                //fillPurchaseHistoryTable();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Login Failed',
                    html: '<p>User Info Not Found</p>'
                })
            }
        
        })
    }
})

function loadMemberQR(MembershipID){
    console.log('Called Load Member QR');
    new QRCode(document.getElementById("divQRcode"),MembershipID);
    $('#spUsername').text(MembershipID);
}
//end of login.html

//start for newAccount.html
$(document).on('click','#btnNewAccount', function(){
    let strErrorMessage='';
    let blnError = false;
    if(!$('#txtNewEmail').val()){
        blnError=true;
        strErrorMessage+= '<p>Email is Blank.</p>';
    }else if(!isValidEmail($('#txtNewEmail').val())){
        blnError=true;
        strErrorMessage+='<p>Email is not valid</p>';
    }
    if(!$('#txtNewPassword').val()){
        blnError=true;
        strErrorMessage+= '<p>Password is Blank.</p>';
    }else if(!isValidPassword($('#txtNewPassword').val())){
        blnError=true;
        strErrorMessage+='<p>Password is not valid</p>';
    }
    else if(!doPasswordsMatch($('#txtNewPassword').val(),$('#txtVerifyPassword').val())){
        blnError=true;
        strErrorMessage+='<p>Passwords do not match</p>';
    }
    if(!$('#txtNewFirstName').val()){
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
    /*if(!$('#txtAddress2').val()){
        blnError=true;
        strErrorMessage+= '<p>Address 2 is Blank.</p>';
    }*/
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
        $.post('http://localhost:7071/api/swollenCoffee?function=membership&Email=' +$('#txtNewEmail').val()+'&FirstName='+$('#txtNewFirstName').val()+'&LastName='+$('#txtLastName').val()+'&PreferredLocation='+$('#cboNewPreferredLocation').val()+'&Password='+$('#txtNewPassword').val()+'&Address1='+$('#txtAddress1').val()+'&Address2='+$('#txtAddress2').val()+'&City='+$('#txtCity').val()+'&State='+$('#txtState').val()+'&ZIP='+$('#txtZIP').val()+'&PhoneNumber='+$('#txtPhoneNumber').val()+'&DOB='+$('#txtDateOfBirth').val(),  function(result){
        let objResult = JSON.parse(result);
        
        if(objResult){
            if(objResult.Outcome.includes('Success')){
                let strMembershipID = objResult.Outcome.split('|')[1];
                let strSessionID = objResult.Outcome.split('|')[2];
                localStorage.setItem('MembershipID',strMembershipID);
                localStorage.setItem('SessionID',strSessionID);
                
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
                window.location.href='login.html';
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Membership Not Created',
                    html: '<p>Something went wrong, we are working to correct it!</p>'
                })
            }
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
        
        $.each(result,(i,location)=>{
            $('#cboNewPreferredLocation').append('<option value="' + location.locationID + '">' + location.locationID + '</option>');
        }) 
    })

//start for index.html
$(document).on('click','#btnSignOut', function(){
    Swal.fire({
        icon: 'error',
        title: 'Sign Out',
        html: 'Are you sure?',
        showCancelButton: true,
        cancelButtonText: 'No',
        showConfirmButton: true,
        confirmButtonText: 'Yes',
    }).then((result)=>{
        if (result.isConfirmed){
            $.ajax({
                method:'DELETE',
                url:'http://localhost:7071/api/swollenCoffee?function=session&SessionID=' + localStorage.getItem('SessionID')
            }).done(function(result){
                console.log(result);
                let objResult = JSON.parse(result);
                if(objResult.Outcome == 'Session Deleted'){
                    localStorage.removeItem('SessionID');
                    window.location.href='login.html';
                }
            })
        }
    })
})

$(document).on('click','#btnToggleExisting', function(){
    $('#divPurchaseHistroy').slideToggle();
})

$(document).on('click','#btnUpdateInformation', function(){
    $.getJSON('http://localhost:7071/api/swollencoffee?function=membership&SessionID='+localStorage.getItem('SessionID'),function(result){
        $.each(result,(i,member)=>{
            $('#txtUpdateAddress').val(member.email);
            $('#txtUpdateFirstName').val(member.firstName);
            $('#txtUpdateLastName').val(member.lastName);
            $('#txtUpdateDateofBirth').val(member.dateOfBirth.split('T')[0]);
            $('#txtUpdatePhoneNumber').val(member.areaCOde + member.telephoneNumber);
            $('#txtUpdatePhoneID').val(member.phoneID);
            $('#txtUpdateAddress1').val(member.street1);
            $('#txtUpdateAddressID').val(member.addressID);
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
    if(!$('#txtUpdateAddress').val()){
        blnError=true;
        strErrorMessage+= '<p>Email is Blank.</p>';
    }else if(!isValidEmail($('#txtUpdateAddress').val())){
        blnError=true;
        strErrorMessage+='<p>Email is not valid</p>';
    }
    if(!$('#txtUpdateFirstName').val()){
        blnError=true;
        strErrorMessage+= '<p>First Name is Blank.</p>';
    }
    if(!$('#txtUpdateLastName').val()){
        blnError=true;
        strErrorMessage+= '<p>Last Name is Blank.</p>';
    }
    if(!$('#txtUpdateAddress1').val()){
        blnError=true;
        strErrorMessage+= '<p>Address 1 is Blank.</p>';
    }
    if(!$('#txtUpdateAddress2').val()){
        blnError=true;
        strErrorMessage+= '<p>Address 2 is Blank.</p>';
    }
    if(!$('#txtUpdateCity').val()){
        blnError=true;
        strErrorMessage+= '<p>City is Blank.</p>';
    }
    if(!$('#txtUpdateState').val()){
        blnError=true;
        strErrorMessage+= '<p>State is Blank.</p>';
    }
    if(!$('#txtUpdateZip').val()){
        blnError=true;
        strErrorMessage+= '<p>Zip Code is Blank.</p>';
    } else if ($('#txtUpdateZip').val().length < 5 || $('#txtUpdateZip').val().length < 5){
        blnError=true;
        strErrorMessage+= '<p>Zip Code Must be 5 Digits Long.'
    }
    if(!$('#txtUpdatePhoneNumber').val()){
        blnError=true;
        strErrorMessage+= '<p>Phone Number is Blank.</p>';
    }
    if(!$('#txtUpdateDateofBirth').val()){
        blnError=true;
        strErrorMessage+= '<p>Birthday is Blank.</p>';
    }
    if(blnError == true){
        Swal.fire({
            icon: 'error',
            title: 'Missing Data',
            html: strErrorMessage
        })
    }else{ // this needs to be a put not a post. +'&PreferredLocation=' + $('#cboNewPreferredLocation').val()
        $.ajax({
            type:'PUT',
            url:'http://localhost:7071/api/swollencoffee?function=membership&Email='+ $('#txtUpdateAddress').val()+'&Firstname='+$('#txtUpdateFirstName').val()+'&Lastname='+ $('#txtUpdateLastName').val()+'&DateOfBirth=' + $('#txtUpdateDateofBirth').val()+'&MembershipID=' + localStorage.getItem('SessionID')+'&PreferredLocation=' + $('#cboNewPreferredLocation').val()+'&PhoneID='+ $('#txtUpdatePhoneID').val()+ '&TelephoneNumber=' + $('#txtUpdatePhoneNumber').val()+'&AddressID='+$('#txtUpdateAddressID').val()+ '&Address1=' + $('#txtUpdateAddress1').val() + '&Address2=' + $('#txtUpdateAddress2').val() + '&City=' + $('#txtUpdateCity').val() + '&State=' + $('#txtUpdateState').val() + '&Zip=' + $('#txtUpdateZip').val() 
        }).done(function(result){
            console.log(result);
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
        })
        $('#divUpdateInfo').slideToggle();
    }
})

$(document).on('click','#btnViewHistory',function(){
    $.getJSON('http://localhost:7071/api/swollenCoffee?function=purchases&SessionID=' + localStorage.getItem("SessionID"),function(result){
        let strHTML = '';
        console.log(result);
       $.each(result,(i,purchase) =>{
           strHTML += '<div class="card mt-2"><div class="card-header"<h3 class="col-12 text-left">' + purchase.transactionDateTime.split('T')[0] + '</h3></div><div class="card-body"><p class="text-left">Item: ' + purchase.description + '</p><p class="text-left">Quantity: ' + purchase.qty  + '</p><p class="text-left">Price: ' + purchase.qtyPrice  + '</p></div></div>'
        })
        Swal.fire({
            icon: 'info',
            title: 'Purchase History',
            html: strHTML
        })
    })
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
            let strTableHTML = '<tr><td> <button type="button" class="btn col-12" id="btnTransaction">  x42ghue78 </button></td></tr>';
            $('#tblPurchaseHistory tbody').append(strTableHTML);
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

