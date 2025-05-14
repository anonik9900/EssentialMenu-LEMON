import xml.etree.ElementTree as ET

tree = ET.parse("PedList.xml")  # Sostituisci con il tuo file XML
root = tree.getroot()

with open("output.ini", "w") as f:
    for category in root.findall("Category"):
        f.write(f"[{category.get('name')}]\n")
        for ped in category.findall("Ped"):
            f.write(f"{ped.get('caption')}={ped.get('name')}\n")
        f.write("\n")  # Spaziatura tra sezioni
