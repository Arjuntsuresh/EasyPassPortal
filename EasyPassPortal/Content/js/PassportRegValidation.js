// Function to set max attribute of date input to today's date
function setMaxDate() {
    let today = new Date().toISOString().split('T')[0];
    document.getElementById("DateOfBirth").setAttribute("max", today);
}

// Function to handle date input changes
function handleDateChange() {
    let selectedDate = new Date(document.getElementById("DateOfBirth").value);
    let today = new Date();

    // If selected date is in the future, reset input value to today's date
    if (selectedDate > today) {
        document.getElementById("DateOfBirth").value = today.toISOString().split('T')[0];
    }
}

// Call setMaxDate function when the page is loaded
window.onload = function () {
    setMaxDate();

    // Add event listener to handle date changes
    document.getElementById("DateOfBirth").addEventListener("input", handleDateChange);
};

// State and District data
let state = {
    kerala: [
        "Alappuzha", "Ernakulam", "Idukki", "Kannur", "Kasaragod",
        "Kollam", "Kottayam", "Kozhikode", "Malappuram", "Palakkad",
        "Pathanamthitta", "Thiruvananthapuram", "Thrissur", "Wayanad"
    ],
    Karnataka: [
        "Bagalkot", "Bangalore Rural", "Bangalore Urban", "Belgaum",
        "Bellary", "Bidar", "Chamarajanagar", "Chikballapur",
        "Chikkamagaluru", "Chitradurga", "Dakshina Kannada",
        "Davanagere", "Dharwad", "Gadag", "Gulbarga", "Hassan",
        "Haveri", "Kodagu", "Kolar", "Koppal", "Mandya", "Mysore",
        "Raichur", "Ramanagara", "Shimoga", "Tumkur", "Udupi",
        "Uttara Kannada", "Vijayapura", "Yadgir"
    ],
    Tamilnadu: [
        "Ariyalur", "Chengalpattu", "Chennai", "Coimbatore",
        "Cuddalore", "Dharmapuri", "Dindigul", "Erode", "Kallakurichi",
        "Kanchipuram", "Kanyakumari", "Karur", "Krishnagiri",
        "Madurai", "Mayiladuthurai", "Nagapattinam", "Namakkal",
        "Nilgiris", "Perambalur", "Pudukkottai", "Ramanathapuram",
        "Ranipet", "Salem", "Sivaganga", "Tenkasi", "Thanjavur",
        "Theni", "Thoothukudi", "Tiruchirappalli", "Tirunelveli",
        "Tirupattur", "Tiruppur", "Tiruvallur", "Tiruvannamalai",
        "Tiruvarur", "Vellore", "Viluppuram", "Virudhunagar"
    ]
};

// Function to change district options based on selected state
function changeValue(value) {
    const citySelect = document.getElementsByName('District')[0];
    if (value.length === 0) {
        citySelect.innerHTML = '<option disabled selected>District</option>';
    } else {
        let options = '<option disabled selected>District</option>';
        for (let id in state[value]) {
            options += "<option>" + state[value][id] + "</option>";
        }
        citySelect.innerHTML = options;
    }
}

function setMaxDate() {
    let today = new Date().toISOString().split('T')[0];
    document.getElementById("DateOfBirth").setAttribute("max", today);
}