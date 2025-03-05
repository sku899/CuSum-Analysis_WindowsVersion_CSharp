# CuSum-Analysis WindowsVersion CSharp #
## Contributor: Sabina Ku

### This CuSum-Analysis is for Surgeon’s Skills Assessment to evaluate surgical performance
### Instruction of CUSUM application
CUSUM Application (CUSUM APP) contains three tabs – ‘Personal’, ‘Training’ and ‘CUSUM’. Their screenshots and functions will be presented and described as the following,
1.	Personal tab: When CUSUM APP starts, ‘Personal’ tab is the starting up screen as shown in figure 1. The other two tabs are disabled and will be enabled after user sign in. 
When sign in, first input GMC number of the user (trainee). Current setup only accepts GMC numbers which have pre-stored in the program. The first digit of pre-stored user (trainee)GMC numbers are 6, 7 or 8. Please input 6, 7 or 8 as the first digit. App will display stored GMC numbers starting from 6, 7 or 8 to be chosen.
In Password field, please use password as the input (for demonstration purpose).
For consultant GMC number, please input 1, 2, 3 or 4 as the first digit of GMC number to get the pre-stored consultant GMC numbers.
Then select the training hospital from hospital list.
![image](https://github.com/user-attachments/assets/b5833f06-20e7-42db-8091-1edf1180af24)
Figure 1: Personal tab.
When the training hospital is selected, the speciality dropdown list in ‘Training’ tab will be loaded. For this demonstration version of APP, the same specialities with the selected hospital as the first item will be loaded.  The specialities are Urology and ENT.
User can change Tab Icon Position, Tab Icon Size and Font Size of Program in ‘Personal’ tab. 
2.	Training tab: The screen shot of ‘Training tab’ is shown in figure 2. In training tab, the user is required to select Specialities and Surgeries. As mentioned above, Specialities dropdown list will contain three items in the following order, name of selected hospital, Urology, ENT. Please select Urology or ENT.
Once Urology or ENT is selected, the corresponding surgeries will be populated to Surgeries dropdown list. For Urology, they are Flexible Cystoscopy, Trans Urethral Resection of Prostate and Circumcision. For ENT, they are Tonsillectomy, Septoplasty and Myringoplasty.
Once surgery is selected, the corresponding surgery skills will be populated into Surgery Skills dropdown list. The existing assessment data will also be loaded into the table and plotted as chart.  Surgery Skills dropdown list, table and chart of assessment data can be seen in ‘CuSum’ tab.
For the demonstration purpose, only surgery Flexible Cystoscopy and Tonsillectomy have data. Please select one of them. 
For other inputs in ‘Training’ tab, such as Patient Information, CEPOD etc. CUSUM APP will accept the inputs but they don’t have any effects on APP.

![image](https://github.com/user-attachments/assets/bd2db8ed-ff4e-4098-92e9-0ee5491e4c06)
Figure 2: Training tab.
3.	CuSum tab: CuSum tab is shown in figure 3. This tab is core of this APP. It can be divided into two parts. The 1st part is the table which lists the loaded assessment data and 2nd part is the the chart of loaded data. Each part contains the data visualisation on the left and operations on the right.  The details will be described next. 

![image](https://github.com/user-attachments/assets/662c5528-c9a7-495c-a614-336fb8b826b6)
Figure 3: CuSum tab.
3.1: Table and its operations. CUSUM APP have the following record operations: (1) Navigate the records, (2) Add new record, (3) Modify record and (4) Delete record as shown in figure 4.
For demonstration purpose, current setup only allows the last record of loaded data and new added records after data loaded to be modified and deleted.

![image](https://github.com/user-attachments/assets/7033d4de-1581-4f48-9e12-bda9f68227ac)
Figure 4: Record operation.
Record can be navigated by scrolling for rapid navigation (gold rectangle in figure 4) or detailed navigation by clicking buttons in dash blue rectangle in figure 4.
To Add or Modify a record, first check ‘Add a New Record’ or ‘Modify an Existing Record’. When one of the checkboxes is checked, user can add or modify the record with two options (1) Overall Result Succeed or (2) Overall Result Not Succeed as shown in Figure 5 (a) dash blue rectangle.  If add a new record is the selected, APP will also automatically to show the new assessment number. If modify an existing record is selected, APP needs user to select the assessment number of record to be modified.
If Overall Result Succeed is selected, Confirm Add (or Modify) Record, will be enabled as shown in Figure 5 (b). In this case, Once the confirm is clicked, APP will automatically add or modify all surgery skills as succeed and calculate the CuSum scores.
If Overall Result Not Succeed is selected, APP will need user to input the individual result for each skill as shown in Figure 5(c). User give the result of the skill by select Succeed or Not Succeed options and clicking ‘Next Skill’ button. Once all skills have results, the confirm button will enabled to add or modify record.
If none of add or modify record is selected, the record can be deleted, as shown in Figure 5 (d). APP needs user to select the assessment number of the record to be deleted.

![image](https://github.com/user-attachments/assets/79692f7f-a4ec-4d80-99f5-2a4b7079f027)
![image](https://github.com/user-attachments/assets/9f1c6725-20fa-4d6d-9a86-0f5c32da79a4)
![image](https://github.com/user-attachments/assets/d1acb48e-3715-4fb6-9169-8937201a0f3e)
![image](https://github.com/user-attachments/assets/2a256b25-7aae-41d9-ad4d-a7ba3ab695a0)
Figure 5: (a) When Add a New Record or Modify an Existing Record is selected. (b) When Overall Result succeed is selected. (c) When Overall Result Not Succeed is selected. (d) when none of  Add a New Record or Modify an Existing Record is selected, the record can be deleted.

3-2: Chart and its operations. Chart has the following options and operations. (1) Show Legend, (2) Display All (display all CuSum plots of all surgeries skills and overall), (3) Navigate Chart and (4) Examine the impact of decision interval h0 as shown in figure 6.
![image](https://github.com/user-attachments/assets/9c4ae809-6cb4-43e6-8a9a-af1b559fe62f)
Figure 6: Chart and its options and operations.
Display All: Chart will display all if its checked or only display the chart of selection in Surgery skills dropdown list.
Navigate Chart: this will display 20 points from the selected starting point. The starting point can be selected by moving the slider bar as shown in Figure 7. Starting point can be changed from 0 to (total number of record – 20).
![image](https://github.com/user-attachments/assets/d45a5e0f-1392-4ab7-993f-9764b5c593f2) 
Figure 7: Navigate (Zoom in) chart by changing the starting point of the chart.
Examine the impact of decision interval h0: this will change the value of h0 to see how it can affect CuSum plot. The value of h0can be changed by moving the slider bar (the circleone) as shown in Figure 8 which can compared to Figure 7 with a different h0. The default value of h0 is 2.94. It can be changed from 1 to 3.
![image](https://github.com/user-attachments/assets/b0f6233c-82dd-4625-af62-06536c2f6800) 
Figure 8: Examine the impact of h0 on CuSum plot.
These four options (Show Legend, Display All, Navigate Chart and Examine the impact of decision interval h0) are independent. The chart can be plotted in any combinations of them.

4.	Save the change: If the records have been added, modified or deleted, the change can be saved by clicking the button ‘save change’ in CuSum tab. Or APP will remind user to change when user attempts to change the data.
