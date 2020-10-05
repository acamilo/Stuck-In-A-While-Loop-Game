motor.right=0.2
motor.left=0.2

if range.front>0.1 
 or range.left>0.1 
 or range.right>0.1 then
	motor.left=-1
	motor.right=0
end

return color.left