PRIORITY_HIGH

if(hasSymptom('Tumora')) log('Naspa de tine');

var symptomsDurere = ['Durere f', 'Durere c'];

for(var i = 0; i < symptomsDurere.length; i++) {
    if(!hasSymptom(symptomsDurere[i])) {
        return false;
    }
}

addNewSymptom('Smecheras');
log("test");
return true;