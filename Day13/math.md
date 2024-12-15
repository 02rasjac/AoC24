# The mathematical solution for Day 13

Assume $\vec{a}$ and $\vec{b}$ being the deltas for button a and b, respectively, and defined as

$$\vec{a} = \begin{bmatrix}x_a \\ y_a\end{bmatrix}, \vec{b} = \begin{bmatrix}x_b \\ y_b\end{bmatrix}, \vec{p} = \begin{bmatrix}x_p \\ y_p\end{bmatrix}.$$

Then, let $n_a$ and $n_b$ be the number of presses for the respective buttons. The equation we want to solve becomes
$$\vec{p} = n_a \vec{a} + n_b \vec{b}.$$
or
$$
\left\{\begin{matrix}
x_p = n_a x_a + n_b x_b \; (1)\\
y_p = n_a y_a + n_b y_b \; (2)
\end{matrix}\right.
$$

Equation (2) gives
$$
n_a = \frac{y_p - n_b y_b}{y_a} \; (3)
$$

(3) in (1) and solving for $n_b$ results in
$$
x_p = x_a \frac{y_p - n_b y_b}{y_a} + n_b x_b \Rightarrow
n_b = \frac{x_p y_a - x_a y_p}{x_b y_a - x_a y_b} \; (4)
$$

By first solving for $n_b$, we can get $n_a$. Since the problem requires $n_a, n_b \in \mathbb{N}$ ( 0, 1, 2, 3, ...),
if $n_b$ or $n_a$ is not an integer from these calculations (found through code by checking the remainder before
dividing) then it's not a valid price. Otherwise, if both are integers, we can calculate to cost.