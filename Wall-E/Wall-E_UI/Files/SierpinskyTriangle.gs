mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);
    
regularTriangle(p,m) =
    let
        point p2;
        l1 = line(p,p2);
        c1 = circle(p,m);
        i1,i2,_ = intersect(l1,c1);
        c2 = circle(i1,m);
        c3 = circle(i2,m);
        i3,i4,_ = intersect(c2,c1);
        i5,i6,_ = intersect(c3,c1);
    in {i1,i5,i6};  
    
divideTriangle(p1,p2,p3,m1) = if (measure(p1,p2)/m1) < 5 then {} else  
   let
      draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};
      mid1,_ = intersect(mediatrix(p1,p2),line(p1,p2));
      mid2,_ = intersect(mediatrix(p2,p3),line(p2,p3));
      mid3,_ = intersect(mediatrix(p1,p3),line(p1,p3));
      a = divideTriangle(p2,mid2,mid1,m1);
      b = divideTriangle(p1,mid1,mid3,m1);
      c = divideTriangle(p3,mid2,mid3,m1);
      in {};
      
sierpinskyTriangle(p,m) = 
     let
         pu1 = point(0,0);
         pu2 = point(0,1);
         p1,p2,p3,_ = regularTriangle(p,m);
     in divideTriangle(p1,p2,p3,measure(pu1,pu2));

pu1 = point(300,0);
pu2 = point(0,0);
m = measure(pu1,pu2);

a = sierpinskyTriangle(point(450,300),m);