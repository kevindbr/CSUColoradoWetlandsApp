Ext.define('CWIC.util.Cwic', {
	singleton: true,
    homeTab: 0,
    plantListTab: 1,
    plantProfileTab: 2,
    filterTab: 3,
    glossaryTab: 4,
    introTab: 5,
    acknowTab: 6,
    helpTab: 7,
    mapsTab: 8,
    mapsHelpTab: 9,
    ecosystemsTab: 10,
    introText: '<span class="subheading">Introduction to the App</span>  <br />' + 
        '<p class="sm">The Colorado Wetlands Mobile App delivers information about Colorado wetlands right to your smartphone or tablet. The app includes two main sections:</p>' +
        '<ul>' +
            '<li class="acknow">' +
                '<span class="sm"><b>Wetland Plants:</b> ' +
                'Detailed descriptions and photos of 710 plant species found in Colorado’s wetlands and riparian areas. Tap Wetland Plants from the Home Screen to search and explore the species descriptions.</span>' +
            '</li>' +
            '<li class="acknow">' +
                '<span class="sm"><b>Wetland Maps:</b> ' +
                'Access to digital National Wetland Inventory maps for Colorado. Tap Wetland Maps from the Home Screen for an interactive mapping tool to explore wetlands mapped in your area. The Wetland Maps screen can also be used to search wetland plants potentially found at your location.</span>' +
            '</li>' +
            '<li class="acknow">' +
                '<span class="sm"><b>Wetland Types:</b> ' +
                'Short descriptions of the major wetland and riparian types in Colorado. The wetland types described in this app are based on NatureServe Ecological Systems, and are modified for Colorado.</span>' +
            '</li>' +
        '</ul>' +
        
        '<br />'+    
        '<span class="subheading">What are Wetlands?</span>  <br />' +
        '<p class="sm">For regulation under the Clean Water Act, the <b>Army Corps of Engineers</b> and <b>Environmental Protection Agency</b> define wetlands based on <em>three</em> criteria: (1) <em>wetland plants, </em> which are adapted to growing in wet areas; (2) <em>wetland soils, </em> which have different properties than dryland soils; and (3) <em>wetland hydrology,</em> meaning the area is wet for at least a couple weeks each year. An area of land must have all three to be considered a wetland. Their official definition is:</p>' +
        '<p class="sm" style="padding-left:10px;"><em>"Those areas that are inundated or saturated by surface or groundwater at a frequency and duration sufficient to support, and that under normal circumstance do support, a prevalence of vegetation typically adapted for life in saturated soil conditions."</em></p>' +
        '<p class="sm">For mapping and habitat management, the <b>U.S. Fish and Wildlife Service</b> define wetlands more broadly. An area can be considered a wetland if it has <em>either</em> wetland plants or undrained wetland soils. This definition recognizes that some areas provide many functions of wetlands without exhibiting all three characteristics required by the Clean Water Act criteria. Their official definition is:</p>' +
        '<p class="sm" style="padding-left:10px;"><em>"Lands transitional between terrestrial and aquatic systems where the water table is usually at or near the surface or the land is covered by shallow water."</em></p>' +

        '<br />'+
        '<span class="subheading">Why are Wetlands Important?</span>  <br />' + 
        '<p class="sm">Colorado is one of the most biologically diverse states in the Intermountain West, with nearly 3,600 plant and animal species. Wetland and riparian areas are transitional lands between terrestrial and aquatic habitats and are among the most diverse ecosystems in the state. Colorado’s wetlands range from alpine wet meadows high in the mountains to marshes on the plains. Though they cover only 2% of the landscape, wetlands and riparian areas are by far the most ecologically and economically significant ecosystem in Colorado.</p>' +
        '<p class="sm">Wetlands provide many functions that are valued by society. These include groundwater recharge, nutrient cycling, primary production, carbon sequestration and export, sediment transport, and channel stabilization. One of the most important functions valued by society is the role of wetlands in providing clean water. Wetland vegetation acts as a filter or sponge for water and sediment that may contain heavy metals, pesticides or fertilizers. Wetland vegetation also provides a buffer for flood zones, especially along larger rivers that flow through Colorado’s cities and towns. In addition, wetlands play a key role in many of the recreational activities Colorado is best known for, including hunting, fishing, wildlife viewing and rafting.</p>' +
      
        '',
    acknowledgeText: '<span class="subheading">Acknowledgments</span>  <br />' + 
        '<p class="sm">The Colorado Wetlands mobile application was funded by a U.S. Environmental Protection Agency, Region 8, Wetland Program Development Grant with oversight from Toney Ott and Cynthia Gonzales. Rebecca Pierce, Wetland Program Manager for Colorado Department of Transportation, and Colorado State University provided matching funds. Content was developed through a series of pervious grants funded by EPA Region 8, Colorado Parks and Wildlife, Colorado Native Plant Society, and Colorado Riparian Association.</p>' +
    '<p class="sm">App content is based on: </p>' +
        '<ul>' +
            '<li class="acknow">' +
                '<span class="sm"><em><b>Field Guide to Colorado\'s Wetland Plants: Identification, Ecology and Conservation</b></em> ' +
                'by Denise Culver and Joanna Lemly. 2013. Colorado Natural Heritage Program.</span>' +
            '</li>' +
            '<li class="acknow">' +
                '<span class="sm"><em><b>Common Wetland Plants of Colorado\'s Eastern Plains: A Pocket Guide</b></em> ' +
                'by Denise Culver. 2014. Colorado Natural Heritage Program.</span>' +
            '</li>' +
            '<li class="acknow">' +
                '<span class="sm"><em><b>National Wetland Inventory</b></em> ' +
                'maps for Colorado developed in partnership with the U.S. Fish and Wildlife Service.</span>' +
            '</li>' +    
        '</ul>' +
    '<p class="sm">App development by Kirstin Holfelder, Colorado Natural Heritage Program. Expert technical assistance provided by Gordon Holfelder. </p>' +
    '<p class="sm">CNHP is a non-profit organization and a research unit within the Warner College of Natural Resources at Colorado State University. CNHP is a member of the NatureServe Network, an international network of natural heritage programs that monitor the status of species and natural communities from state, national, and global perspectives. The mission of CNHP is:</p>' +
    '<p class="sm" style="padding-left:10px;"><em>"To preserve the natural diversity of life by contributing the essential scientific foundation that leads to lasting conservation of Colorado’s biological wealth."</em></p>' +
    '<p class="sm"><a href="http://www.cnhp.colostate.edu">CNHP’s webpage, http://www.cnhp.colostate.edu</a></p>' +

    '<img src="resources/icons/EPA.gif" height="100px;" />' +
    '<img src="resources/icons/CNHP.png" height="100px;" />' +
    '<img src="resources/icons/CSU.png" height="100px;" />' +
    '<img src="resources/icons/CDOT.png" height="100px;" />' +
    '<img src="resources/icons/CPW.png" height="100px;" />' +
    '<img src="resources/icons/CoNPS.gif" height="100px;" />' +
    '<img src="resources/icons/CRA.png" height="100px;" />' +
    '<img src="resources/icons/USFWS.gif" height="100px;" />' +
    '<img src="resources/icons/NatureServe.png" height="100px;" />' +
        '',
    helpText: '<span class="subheading">Help Using Wetland Plants</span>  <br />' + 
    '<p class="sm">The Wetland Plants Section of the app contains detailed information on 710 vascular plant species found in Colorado wetlands and riparian areas. The list can be sorted by Scientific Name, Common Name, Family, and Group. The list can be searched in many ways using the Search button from the main Plants Screen. Access a glossary of botanical terms by tapping the Terms button on the main Plants Screen. </p>' +
    '<p class="sm">To view the full description of a plant, tap the entry from either the full list or the results of a search. Each description is broken into six pages, which are described below. You can move between pages using buttons at the bottom of the screen. </p>' +    
   
   
    //Images
    '<div class="lv1">' +
        '<span class="heading">Images</span><br />' + 
    '</div>'+ 
    '<p class="sm">Each species includes photos and/or illustrations that highlight the most diagnostic characteristics of the plant. Images can be browsed by swiping to the left or right. Images were compiled from numerous different sources, including Colorado photographers, internet-based photo databases, genera-specific photo collections of herbarium specimens, and botanical illustrators.</p>' +
    
    //General 
    '<div class="lv1">' +
        '<span class="heading">General Description</span><br />' + 
    '</div>'+ 
    '<p class="sm">The General Description page includes a range of information relevant to each species, as defined below. </p>' +
    '<p class="sm"><b>Scientific Name:</b>  USDA-NRCS PLANTS National Database is the primary nomenclature used for scientific names.  This nomenclature differs in some instances from state-based floras (e.g., Weber and Wittmann 2012, Ackerfield 2012), but is best for comparing across state borders and between various national datasets. </p>' +
    '<p class="sm"><b>Family:</b> Family names used in the app are derived from PLANTS National Database. If a species is treated in a different family in one of the state floras or in Flora of North America, the alternate family name is listed in parenthesis.</p>' + 
    '<p class="sm"><b>Common Name:</b> Common names are generally derived from PLANTS National Database. In cases where there is more than one common name, both are listed. </p>' + 
    '<p class="sm"><b>Synonyms:</b> Major synonyms are listed for each species. A special effort was made to include all names used by Weber and Wittmann (2012), Ackerfield (2012), and the most recent Flora of North America treatments (Flora of North America 1993+).</p>' + 
    '<p class="sm"><b>USDA PLANTS Symbol:</b> The unique alpha-numeric symbol for each species used within PLANTS National Database. The symbols begin with the first two letters of the genus name and the first two letters of the species name, followed by the first letter of the subspecies or varieties, if applicable. If the letters in any code are the same for more than one taxon, a number is included at the end of the code to make each code unique.</p>' + 
    '<p class="sm"><b>ITIS TSN:</b> The Integrated Taxonomic Information System (ITIS) Taxonomic Serial Number (TSN). Like the USDA PLANTS Symbol, this is a unique numeric code used to differentiate species and is used by many national and international agencies.</p>' + 
    '<p class="sm"><b>Federal Status:</b> Notes whether a species is listed as threatened under the federal Endangered Species Act or considered sensitive by the Bureau of Land Management (BLM) or U.S. Forest Service (USFS).</p>' +
    '<p class="sm"><b>Global / State Ranks:</b> From the Natural Heritage Network, which ranks the rarity of species on a Global (G) and Subnational/State (S) level. Basic ranks are shown in the table below. State ranks are included for all six states within EPA Region 8 (CO, WY, UT, MT, SD, ND).</p>' +
    
    '<table class="introTable">' +
                        '<th class="introTable">' +
                            
                                '<span class="sm"><b>Rank</b></span>' + 
                        '</th>' +
                        '<th class="introTable">' +
                                '<span class="sm"><b>Interpretation</b></span>' + 
                            
                        '</th>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">1</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Critically Imperiled (typically 5 or fewer occurrences or less than 1,000 individuals)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">2</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Imperiled (typically 6 to 20 occurrences or between 1,000 and 3,000 individuals)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">3</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Vulnerable to Extirpation (typically 21 to 100 occurrences or between 3,000 and 10,000 individuals)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">4</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Apparently Secure (usually more than 100 occurrences and more than 10,000 individuals)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">5</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Demonstrably Widespread, Abundant, and Secure (typically with considerably more than 100 occurrences and more than 10,000 individuals)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">NR</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Not Ranked (not enough information is available on which to base a rank)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">NA</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Not Applicable (rarity ranking is not applicable because the species is not native to the state)</span>' + 
                            '</td>' +
                        '</tr>' +
    '</table>' +
    '<br />' +
    '<p class="sm"><b>C-Value:</b> The C-Value is the "coefficient of conservatism," which represents the estimated probability that a species occurs in a landscape that is either pristine or disturbed. C-values range from 0–10 with 0 reserved for non-native species. See table below for an explanation of C-values and example species. The average C-value of a plant community assesses the degree of “naturalness” based on the presence or absence of conservative species and provides a powerful and relatively easy assessment of biotic integrity. C-values for Colorado species were assigned by a panel of botanical experts.</p>' +
    '<table class="introTable">' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>C-Values</b></span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Interpretation</b></span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Examples (C-Value)</b></span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">0</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Non-native species. Very prevalent in new ground or non-natural areas.</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Watercress (<em>Nasturtium officinale</em>) (0)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">1-3</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Commonly found in non-natural areas.</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Water plantain (<em>Alisma trivale</em>) (3)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">4-6</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Equally found in natural and non-natural areas.</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Woolly sedge (<em>Carex pellita</em>) (6)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">7-9</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Obligate to natural areas but can sustain some habitat degradation.</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Blue-eyed grass (<em>Sisyrinchium pallidum)</em> (7)</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">10</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Obligate to high quality natural areas (relatively unaltered from pre-European settlement).</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Round-leaf sundew (<em>Drosera rotundifolia)</em> (10)</span>' + 
                            '</td>' +
                        '</tr>' +
    '</table>' + 
    '<br />' +
    '<p class="sm"><b>Duration:</b> Indicates if a species is typically annual, biennial, perennial, or some combination of the three. This information is derived from PLANTS National Database.</p>' +
    '<p class="sm"><b>Native Status:</b> Denotes whether a plant is considered native, non-native, or, in limited cases, both native and non-native. Native status used in this guide is derived from PLANTS National Database, which largely considers whether a plant is native to the contiguous United States. There is considerable debate among taxonomic experts on the origin of certain plant species. Where there is debate about whether a species is native to Colorado, we have included that information in comments on the Ecology page.</p>' + 
    '<p class="sm"><b>Weed Status:</b> Notes all species on the Noxious Weed List A, B and C and the Watch List as designated by the Colorado Department of Agriculture.' +
    '<p class="sm"><b>Wetland Status:</b> The likelihood that a particular plant occurs in a wetland or upland. Wetland indicator ratings are determined by the U.S. Army Corps of Engineers and are specific to the three regions within Colorado (AW: Arid West, WM: Western Mountains, GP: Great Plains). See map and table below for an explanation of each rating.	</p>' + 
    '<img src="resources/images/WetlandRegions.jpg" width="275px"/> ' +
        '<table class="introTable">' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Indicator Code</b></span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Indicator Status</b></span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Comment</b></span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">OBL</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Obligate Wetland</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Almost always occurs in wetlands.</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">FACW</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Facultative Wetland</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Usually occurs in wetlands, but may occur in non-wetlands.</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">FAC</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Facultative</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Occurs in wetland and non-wetlands.</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">FACU</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Facultative Upland</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Usually occurs in non-wetlands, but may occur in wetlands.</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">UPL</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Obligate Upland</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Almost never occurs in wetlands.</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm">NI</span>' +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">No Indicator</span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Insufficient information available to determine indicator status.</span>' + 
                            '</td>' +
                        '</tr>' +
                    '</table><br />' +
    '<p class="sm"><b>Key Characteristics:</b> Perhaps the most important part of this page, the key characteristics include up to five bullets that detail the most important and distinguishing characteristics of the species. In general, the first bullet describes overall plant size, plant habit, stem characteristics, and rooting structure. The second bullet describes the most important features of the leaves, including the size, shape, position on the plant, presence of hairs, etc. If there is more than one type of leaf, both are described in detail. Remaining bullets describe important features of the inflorescence, flowers and flower parts, and seeds. The key characteristics vary by family and genus, as each has particular characteristics of importance.</p>' +
    
    '<a name="similar"></a> ' +
    //Similar
    '<div class="lv1">' +
        '<span class="heading">Similar Species</span><br />' + 
    '</div>'+ 
    '<p class="sm">The Similar Species page includes look-alikes that could be easily mistaken for the main species. If a similar species has a full description in the App, it will appear in an interactive list. If it is not included in the app, it is described in brief in General Comments above the list and the USDA PLANTS Code, Wetland Indicator Status, and ITIS TSN are provided. General Comments may also include distinguishing characteristics of two or more subspecies or distinguishing characteristics of the entire genus.</p>' +
    
    //Ecology
    '<div class="lv1">' +
        '<span class="heading">Ecology</span><br />' + 
    '</div>'+ 
    '<p class="sm">The Ecology page includes information about the habitat and ecological preferences of the species, as well as general comments and icons representing animals groups that typically use the species.</p>' +

    '<p class="sm"><b>Habitat and Ecology:</b> The general habitat and ecology of the species, including the general region of the state where it occurs. If the species is known only from a handful of counties, they are mentioned specifically.</p>' +
    '<p class="sm"><b>Comments:</b> Comments includes a range of noteworthy information about the species. This information includes facts about wildlife use, ethnobotanical use, origins of the plant name, and evolutionary strategies of the plant or plant family. In cases where there is debate over nomenclature or over whether the plant is native to Colorado, that information is also included in comments.</p>' +
    '<p class="sm"><b>Ecological Systems:</b> A list of Ecological Systems (wetland types) in which the species is likely to occur. Ecological Systems are dynamic assemblages or complexes of plant communities that occur together on the landscape are tied together by similar ecological processes, underlying abiotic environmental factors or gradients and form a readily identifiable unit on the ground.</p>' + 
    '<p class="sm"><b>Animal Use:</b> Along with animal use described in comments, the animal use icons are a quick reference for which animal and bird groups use each plant species.</p>' +
    '<table class="introTable">' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Symbol</b></span>' + 
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm"><b>Animals</b></span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Gruiformes.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Birds: Cranes & Rails</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Galliformes.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Birds: Game Fowl</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Charadriiformes.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Birds: Gulls</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Podicipediformes.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Birds: Grebes</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Passeriformes.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Birds: Passerines></span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Anseriformes.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Birds: Waterfowl</span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Cervidae.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Elk, Deer, Moose </span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Muridae.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Muskrat, Beaver </span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/AmphibiansReptiles.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Amphibians, Reptiles </span>' + 
                            '</td>' +
                        '</tr>' +
                        '<tr class="introTable">' +
                            '<td class="introTable">' +
                                '<img src="resources/images/animals/Insects.jpg" />'  +
                            '</td>' +
                            '<td class="introTable">' +
                                '<span class="sm">Insects </span>' + 
                            '</td>' +
                        '</tr>' +
    '</table>' +
    
    //Range
    '<div class="lv1">' +
        '<span class="heading">Range</span><br />' + 
    '</div>'+ 
  
    '<p class="sm">The Range page includes a Colorado county distribution map and the species’ known elevation range in Colorado. County distribution and elevation data were derived from a thorough compilation of herbarium records, CNHP data, information within literature, and additional information from reviewers.</p>' +
    
    //References
    '<div class="lv1">' +
        '<span class="heading">References</span><br />' + 
    '</div>'+ 
    '<p class="sm">References include all sources used to write a particular species description.</p>' +

    '',
    mapsHelpText: '<span class="subheading">Help Using the Wetland Maps Section</span><br />' + 
        '<p class="sm">The Wetland Maps Section allows users to interactively explore up-to-date digital National Wetland Inventory (NWI) mapping for Colorado. When you first enter the mapping screen, your device will center the mapping at your general location. You can zoom in or out using either your finger or the buttons on the top left portion of the screen. You can also move the view to any location in Colorado. To reset the view, simply click the refresh button. At the bottom of the screen, the Key button will bring up a key to the NWI codes used for labeling polygons. The Legend button will bring up a color coded legend to the mapping’s color scheme. </p>' +
        '<p class="sm">One of the best features of the Wetland Mapping section is the Plant button. Clicking this button will bring up a list of wetland plants potentially found in your general location, as determined by county and current elevation +/- 500 feet. The list is filtered based on your GPS location, not the extent of your view, so remember that if you move the view away from your current location. The filtered plant list allows users to become familiar with common wetland plants found in their neighborhood or when out hiking! </p>' +
            '<span class="subheading"> Background on NWI Mapping </span>  <br />' +
        '<p class="sm">The National Wetland Inventory (NWI) has been producing wetland maps and geospatial wetland data for the United States since the mid-1970s. NWI is headed by the U.S. Fish and Wildlife Service and relies on contributions from many collaborators to produce high quality wetland mapping from the nation. Colorado Natural Heritage Program (CNHP) and Colorado Parks and Wildlife (CPW) have worked closely with NWI since 2008 to create a complete digital map of wetlands for Colorado. Much of the mapping displayed in this app was originally created in the 1970s and 1980s as paper maps based on coarse air photography and only recently converted to digital data by CNHP and CPW. However, CNHP has created updated mapping in many areas of the state, included the northern Front Range and Park County.</p>' +
    
        '<p class="sm">For more information, please see: </p>' +
    
        '<ul>' +
            '<li class="acknow">' +
                'NWI home page: http://www.fws.gov/wetlands/index.html' +
            '</li>' +
            '<li class="acknow">' +
                'CNHP’s webpage on mapping: http://www.cnhp.colostate.edu/cwic/location.asp' +
            '</li>' +
        '</ul>' +
        '',
    
	generalTemplate: null,
    similarTemplate: null,
    ecologyTemplate: null,
    rangeTemplate: null,
    /**
     * Create templates to be used elsewhere
     */
    createTemplates: function(){
        var me = this;
        me.generalTemplate = new Ext.XTemplate(
            '<span class="sm"><div class="lv1">' +
                '<span class="heading">Nomenclature</span><br />' + 
            '</div>' +
            
            '<div class="header">' +
            
                '<span class="subheading">Scientific Name:</span> {SciNameAuthor}<br />' + 
                '<span class="subheading">Family:</span> {Family} <br />' + 
                '<span class="subheading">Common Name:</span> {CommonName}<br />' +
                '<span class="subheading">Synonyms:</span> <tpl for="." if="Synonyms==\'\'">None</tpl>{Synonyms}<br />' +
                '<span class="subheading">USDA Plants Symbol:</span> {PLANTSCode} <br />' +
                '<span class="subheading">ITIS TSN:</span> {ITISCode} <br />' +                    
            '</div>'+ 

            '<div class="lv1">' +
                '<span class="heading">Conservation Status</span><br />' + 
            '</div>'+ 
            
            '<div class="lv2">' +            
                '<span class="subheading">Federal Status:</span> ' +
                
                //If federal status is blank, display a dash
                '<tpl for="." if="FederalStatus==\'\'">-</tpl>' +
                //Else, just write the federal status rank
                '{FederalStatus} <br />' + 
            
            '<span class="subheading">Global Rank:</span> {GRank} <br />' + 

            '<span class="subheading">State Ranks</span><br />' + 
                '<div class="lv2">' +
                    '<table style="width:100%;padding-left:10px;">' +
                        '<tr>' +
                            '<td>' +
                                '<span class="subheading">CO:</span> {COSRank} <br />' + 
                            '</td>' +
                            '<td>' +
                                '<span class="subheading">MT:</span> <tpl for="." if="MTSRank==\'\'">-</tpl>{MTSRank} <br />' +  
                            '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td>' +
                                '<span class="subheading">WY:</span> <tpl for="." if="WYSRank==\'\'">-</tpl>{WYSRank} <br />' +  
                            '</td>' +
                            '<td>' +
                                '<span class="subheading">ND:</span> <tpl for="." if="NDSRank==\'\'">-</tpl>{NDSRank} <br />' +                                 
                            '</td>' +
                        '</tr>' +
                        '<tr>' +
                            '<td>' +
                                '<span class="subheading">UT:</span> <tpl for="." if="UTSRank==\'\'">-</tpl>{UTSRank} <br />' + 
                            '</td>' +
                            '<td>' +
                            '</td>' +
                        '</tr>' +
                    '</table>' +
                '</div>'+ 
            '</div>'+ 
             
            '<div class="lv1">' +
                '<span class="heading">Biology</span><br />' + 
            '</div>'+    
            
            '<div class="lv2">' +
            
                '<span class="subheading">C-Value:</span> {CValue} <br />' +
                '<span class="subheading">Duration:</span> {Duration} <br />' + 
                '<span class="subheading">Native Status:</span> {Nativity} <br />' + 
                
                '<tpl for="." if="NoxiousWeed!=\'\'"><span class="subheading">Weed Status:</span> {NoxiousWeed}<br /></tpl>' +
            
                '<span class="subheading">Wetland Indicator Status</span>' + 
                
                '<div class="lv2">' +
                    '<span class="subheading">AW:</span> {AWWetCode} <br />' +
                    '<span class="subheading">WM:</span> {WMVCWetCode} <br />' +        
                    '<span class="subheading">GP:</span> {GPWetCode} <br />' +
                '</div>'+
                
            '</div>'+
            
            
            '<div class="lv1">' +
                '<span class="heading">Key Characteristics</span>' + 
            '</div>'+
            
            
            '<ul>' +
            
                '<tpl for="." if="keychar1!=\'\'"><li class="keychars">{keychar1}</li></tpl>' +
                '<tpl for="." if="keychar2!=\'\'"><li class="keychars">{keychar2}</li></tpl>' +
                '<tpl for="." if="keychar3!=\'\'"><li class="keychars">{keychar3}</li></tpl>' +
                '<tpl for="." if="keychar4!=\'\'"><li class="keychars">{keychar4}</li></tpl>' +
                '<tpl for="." if="keychar5!=\'\'"><li class="keychars">{keychar5}</li></tpl>' +
            
            '</ul></span>');
        
        me.similarTemplate = new Ext.XTemplate('<span class="sm"><div class="header"><span class="subheading">General Comments:</span> ' +
                                              '<tpl for="." if="SimilarSp==\'\'">None</tpl>{SimilarSp}</div></span>');
        
        me.ecologyTemplate = new Ext.XTemplate('<span class="sm"><span class="subheading">Habitat & Ecology:</span> {Habitat} <br /><br />' +
                                              '<span class="subheading">Comments:</span> {Comments} <br /><br />' +
                                              '<span class="subheading">Wetland Types:</span><tpl for="." if="EcologicalSystems==\'\'"> None</tpl> {EcologicalSystems}<br />' +
                                              '<span class="subheading">Animal Use:</span> ' +
                                              '<tpl for="." if="AnimalUse==\'\'">None</tpl><br />{AnimalUse}</span>' 
                                             );
        me.rangeTemplate = new Ext.Template('<img src="resources/images/maps/{MapImg}" width="100%"><br />' +
                                            '<span class="sm">Elevation: {ElevMinFeet}-{ElevMaxFeet} ft. ({ElevMinM}-{ElevMaxM} m)</span>');
     
        //XTemplates for the list order
        me.sortSciNameTemplate = new Ext.Template('<table> ' +
            '<tr>' +
            '    <td>' +
            '       <img src="resources/images/{TopImg}" class=\'listImg\'>' +
            '    </td>' +
            '    <td class="listName">' +
            '    <strong>{SciNameNoAuthor}</strong> <br />' +
            '    {CommonName}<br />' +       
            '    {Family}<br />' +
            '    {Sections}' +
            '    </td>' +
            '</tr>' +
            '</table>');
        
        me.sortCommonTemplate = new Ext.Template('<table> ' +
            '<tr>' +
            '    <td>' +
            '       <img src="resources/images/{TopImg}" class=\'listImg\'>' +
            '    </td>' +
            '    <td class="listName">' +
            '    <strong>{CommonName}</strong> <br />' +
            '    {SciNameNoAuthor}<br />' +       
            '    {Family}<br />' +
            '    {Sections}' +
            '    </td>' +
            '</tr>' +
            '</table>');
        
        me.sortFamilyTemplate = new Ext.Template('<table> ' +
            '<tr>' +
            '    <td>' +
            '       <img src="resources/images/{TopImg}" class=\'listImg\'>' +
            '    </td>' +
            '    <td class="listName">' +
            '    <strong>{Family}</strong><br />' +
            '    {SciNameNoAuthor}<br />' +   
            '    {CommonName} <br />' +
            '    {Sections}' +
            '    </td>' +
            '</tr>' +
            '</table>');
        
        me.sortSectionsTemplate = new Ext.Template('<table> ' +
            '<tr>' +
            '    <td>' +
            '       <img src="resources/images/{TopImg}" class=\'listImg\'>' +
            '    </td>' +
            '    <td class="listName">' +
            '    <strong>{Sections}</strong> <br />' +                                                   
            '    {SciNameNoAuthor}<br />' +   
            '    {CommonName} <br />' +
            '    {Family}' +
            '    </td>' +
            '</tr>' +
            '</table>');
        
    }
});