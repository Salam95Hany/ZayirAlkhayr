import { Component } from '@angular/core';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-final-profits',
  templateUrl: './final-profits.component.html',
  styleUrls: ['./final-profits.component.css']
})
export class FinalProfitsComponent {
  kpis = [
    {
      title: 'Total Revenue',
      value: '$45,231',
      percent: 18.6,
      icon: 'bi-currency-dollar',
      bg: '#ecfdf5',
      color: '#10b981',
      sparkId: 'spark1'
    },
    {
      title: 'Total Cost',
      value: '$27,430',
      percent: 12.4,
      icon: 'bi-box-seam',
      bg: '#fffbeb',
      color: '#f59e0b',
      sparkId: 'spark2'
    },
    {
      title: 'Net Profit',
      value: '$17,801',
      percent: 24.7,
      icon: 'bi-graph-up-arrow',
      bg: '#eef2ff',
      color: '#6366f1',
      sparkId: 'spark3'
    }
  ];

  ngAfterViewInit() {
    this.initTrendChart();
    this.initSparklines();
  }

  initSparklines() {
    const data = [10, 20, 15, 30, 25, 35];

    this.createSpark('spark1', data, '#10b981');
    this.createSpark('spark2', data, '#f59e0b');
    this.createSpark('spark3', data, '#6366f1');
  }

  createSpark(id: string, data: number[], color: string) {
    new Chart(id, {
      type: 'line',
      data: {
        labels: data,
        datasets: [{
          data,
          borderColor: color,
          tension: 0.4,
          pointRadius: 0,
          fill: true
        }]
      },
      options: {
        plugins: { legend: { display: false } },
        scales: { x: { display: false }, y: { display: false } }
      }
    });
  }

  initTrendChart() {
    new Chart('trendChart', {
      type: 'line',
      data: {
        labels: Array.from({ length: 12 }, (_, i) => `Day ${i + 1}`),
        datasets: [{
          data: [100, 200, 300, 250, 400, 500, 450, 600, 700, 650, 800, 900],
          borderColor: '#6366f1',
          fill: true,
          tension: 0.4
        }]
      },
      options: {
        plugins: { legend: { display: false } }
      }
    });
  }
}
