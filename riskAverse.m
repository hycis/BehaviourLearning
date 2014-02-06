name = 'riskAverse'
filename = strcat('../',name, '_filteredTrace_a.out');
delimiterIn = ':';
headerlinesIn = 0;
A = importdata(filename, delimiterIn, headerlinesIn);
Y = A(:,3)


N = length(Y);
% model = arima('Constant',0,'D',1,'Seasonality',60,...
%               'MALags',5,'SMALags',8);
          
          model = arima('Constant',0,'D',1,'Seasonality',60,...
              'MALags',5,'SMALags',8);
fit = estimate(model,Y);

[Yf,YMSE] = forecast(fit,60,'Y0',Y);
upper = Yf + 1.96*sqrt(YMSE);
lower = Yf - 1.96*sqrt(YMSE);

[nrows, ncols] = size(Yf)

filename = strcat('../', name, '_forecast.in');

fid = fopen(filename, 'w');

for row=1:nrows
    fprintf(fid, '%d\n', Yf(row));
    Yf(row)
end

fclose(fid);

% save forecast.txt Yf

figure
plot(Y,'Color',[.75,.75,.75])
hold on
h1 = plot(N+1:N+60,Yf,'r','LineWidth',2);
h2 = plot(N+1:N+60,upper,'k--','LineWidth',1.5);
plot(N+1:N+60,lower,'k--','LineWidth',1.5)
xlim([0,N+60])
title('Risk Averse')
legend([h1,h2],'Forecast','95% Interval','Location','NorthWest')
xlabel('time')
ylabel('danger value')
hold off