IsMandelBrot(a,b,R,I,m)=
if(m<=0)
then 
	(if ((a*a)+(b*b)<4) 
	then
		 1 
	else 0)
else
 	IsMandelBrot(((a*a)-(b*b)+R),(2*a*b+I),R,I,(m-1));

MandelBrot01(x,y,R,I)= 
if (IsMandelBrot(x,y,R,I,30))	
then 
	let
		draw point(x*100+350,y*100+350);
		M1=MandelBrot01(x+0.01,y,R+0.01,I);
	in 1
else 0;

MandelBrot11(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M1=MandelBrot11(x-0.01,y,R-0.01,I);
in 1
else 0;

MandelBrot02(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M1=MandelBrot02(x,y+0.01,R,I+0.01);
in 1
else 0;

MandelBrot22(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M22=MandelBrot22(x,y-0.01,R,I-0.01);
in 1
else 0;

MandelBrot3(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M3=MandelBrot3(x,y+0.01,R,I+0.01);
M01=MandelBrot01(x,y+0.01,R,I+0.01);
M11=MandelBrot11(x,y+0.01,R,I+0.01);
in 1
else 0;

MandelBrot4(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M4=MandelBrot4  (x,y-0.01,R,I-0.01);
M01=MandelBrot01(x,y-0.01,R,I-0.01);
M11=MandelBrot11(x,y-0.01,R,I-0.01);
in 1
else 0;

MandelBrot5(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M5=MandelBrot5  (x+0.01,y,R+0.01,I);
M02=MandelBrot02(x+0.01,y,R+0.01,I);
M22=MandelBrot22(x+0.01,y,R+0.01,I);
in 1
else 0;

MandelBrot6(x,y,R,I)= if (IsMandelBrot(x,y,R,I,30))
then let
draw point(x*100+350,y*100+350);
M1=MandelBrot6 (x-0.01,y,R-0.01,I);
M2=MandelBrot02(x-0.01,y,R-0.01,I);
M3=MandelBrot22(x-0.01,y,R-0.01,I);
in 1
else 0;


k01= MandelBrot01(0,0,0,0);
k11= MandelBrot11(0,0,0,0);
k02= MandelBrot02(0,0,0,0);
k22= MandelBrot22(0,0,0,0);
k3= MandelBrot3(0,0,0,0);
k4= MandelBrot4(0,0,0,0);
k5= MandelBrot5(0,0,0,0);
k6= MandelBrot6(0,0,0,0);
